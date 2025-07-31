using ClientManager.OpenAI;
using Process.Interface;
using Prompt.Managers.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prompt.Managers
{
    public class PromptManager : IPromptManager
    {
        private readonly IOpenAIManager _openAIManager;
        private readonly IProcessPDF _processpdf;
        public PromptManager(IOpenAIManager openApi, IProcessPDF processPDF)
        {
            _openAIManager = openApi;
            _processpdf = processPDF;
        }
        public Task<float[]> GetEmbeddingAsync(string text)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, float[]>> GetEmbeddingsForLargeTextAsync(string text, int maxChunkSize = 4000)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            var chunks = await _processpdf.GetQueryChunks(prompt);
            // Assuming _openAIManager has a method to process the prompt
            var response = await _openAIManager.GetResponseAsync(prompt, chunks);
            return response;
        }
    }
}
