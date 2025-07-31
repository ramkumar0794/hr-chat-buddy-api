using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManager.PineCone
{
    public interface IPineConeClient
    {
        /// <summary>
        /// Sends a prompt to the PineCone API and retrieves the response.
        /// </summary>
        /// <param name="prompt">The prompt to send to the PineCone API.</param>
        /// <returns>The response from the PineCone API.</returns>
        Task<List<string>> GetQueryEmbeddings(float[]? promptEmbedding);
        Task UpsertEmbeddings(string chunkText, float[] embedding, string id);
    }
}
