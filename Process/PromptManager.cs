using ClientManager.OpenAI;
using ClientManager.PineCone;
using Process.Interface;

namespace Process
{
    public class PromptManager : IPromptManager
    {
        private readonly IOpenAIManager _openAIManager;
        private readonly IProcessPDF _processpdf;
        private readonly IPineConeClient _pineconeClient;
        public PromptManager(IOpenAIManager openAIManager, IProcessPDF processpdf, IPineConeClient pineConeClient)
        {
            _openAIManager = openAIManager ?? throw new ArgumentNullException(nameof(openAIManager));
            _processpdf = processpdf ?? throw new ArgumentNullException(nameof(processpdf));
            _pineconeClient = pineConeClient ?? throw new ArgumentNullException(nameof(pineConeClient));
        }
        public async Task ProcessPDFChunksAsync(string? tools = null)
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, "Files/hrpolicy.pdf");
            var text = _processpdf.ExtractText(filePath);
            if (!string.IsNullOrWhiteSpace(tools) && tools == "pine")
            {
                var chunkEmbeddings = await _openAIManager.GetEmbeddingsForLargeTextAsync(text);
                foreach (var kvp in chunkEmbeddings)
                {
                    var chunkText = kvp.Key;
                    var embedding = kvp.Value;
                    await _pineconeClient.UpsertEmbeddings(chunkText, embedding, Guid.NewGuid().ToString());
                }
            }
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            var promptEmbedding = await _openAIManager.GetEmbeddingAsync(prompt);
            var semanticSearchEmbeddings = await _pineconeClient.GetQueryEmbeddings(promptEmbedding);
            // Assuming _openAIManager has a method to process the prompt
            var response = await _openAIManager.GetResponseAsync(prompt, semanticSearchEmbeddings);
            return response;
        }
    }
}
