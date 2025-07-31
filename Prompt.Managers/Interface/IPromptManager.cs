using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prompt.Managers.Interface
{
    public interface IPromptManager
    {
        Task<string> GetResponseAsync(string prompt);
        Task<float[]> GetEmbeddingAsync(string text);
        
        Task<Dictionary<string, float[]>> GetEmbeddingsForLargeTextAsync(string text, int maxChunkSize = 1000);
    }
}
