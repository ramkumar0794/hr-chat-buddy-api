using Microsoft.AspNetCore.Mvc;
using Process.Interface;

namespace PromptLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromptController : ControllerBase
    {
        private readonly IPromptManager _promptManager;
        public PromptController(IPromptManager promptManager)
        {
            _promptManager = promptManager;
        }
        [Route("CreateResponse")]
        [HttpPost]
        public async Task<IActionResult> CreateResponse(string prompt)
        {
            if(string.IsNullOrEmpty(prompt))
            {
                return BadRequest("Question cannot be null or empty.");
            }
            try
            {
                var response = await _promptManager.GetResponseAsync(prompt);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception (logging not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        [Route("ProcessPDFChunksAsync")]
        public async Task<IActionResult> ChunkPDF()
        {
            try
            {
                await _promptManager.ProcessPDFChunksAsync();
                return Ok("PDF chunking completed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception (logging not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
