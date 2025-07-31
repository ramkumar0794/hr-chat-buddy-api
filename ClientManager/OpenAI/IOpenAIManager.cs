using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManager.OpenAI
{
    public interface IOpenAIManager
    {
        /// <summary>
        /// Sends a prompt to the OpenAI API and retrieves the response.
        /// </summary>
        /// <param name="prompt">The prompt to send to the OpenAI API.</param>
        /// <returns>The response from the OpenAI API.</returns>
        Task<string> GetResponseAsync(string prompt, List<string> contextChunks);
        Task<float[]> GetEmbeddingAsync(string text);
        Task<Dictionary<string, float[]>> GetEmbeddingsForLargeTextAsync(string text, int maxChunkSize = 1000);
    }
}
