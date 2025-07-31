using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Interface
{
    public interface IPromptManager
    {
        /// <summary>
        /// Sends a prompt to the OpenAI API and retrieves the response.
        /// </summary>
        /// <param name="prompt">The prompt to send to the OpenAI API.</param>
        /// <returns>The response from the OpenAI API.</returns>
        Task<string> GetResponseAsync(string prompt);
        /// <summary>
        /// Chunks a PDF file into smaller parts.
        /// </summary>
        /// <param name="filePath">The path to the PDF file.</param>
        /// <param name="chunkSize">The size of each chunk.</param>
        /// <param name="outputDirectory">The directory where chunks will be saved.</param>
        /// <param name="tools">Additional tools or parameters for processing.</param>
        Task ProcessPDFChunksAsync(string? tools = null);
    }
}
