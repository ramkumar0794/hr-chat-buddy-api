using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace ClientManager.OpenAI
{
    public class OpenAIManager : IOpenAIManager
    {
        // This class is responsible for managing OpenAI API interactions.
        // It will handle API requests, responses, and any necessary configurations.
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "7SoDuyJTFUzXSdfMKotEPWel23wag5WAjHULdASDraZn1YrQmSFsJQQJ99BGACYeBjFXJ3w3AAABACOGHZpN";
        public OpenAIManager()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<string> GetResponseAsync(string prompt,List<string> contextChunks)
        {
            var httpClient = Initialize("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");
            var systemPrompt = "Answer the user's question only using the context below. If the answer is not present, say 'Not found in HR policies'.\n\nContext:\n" + string.Join("\n\n", contextChunks);
            var payload = new
            {
                model = "gpt-4o-mini",
                store = true,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role ="user",content = prompt}
                }
            };
            var response = await httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", payload);
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        private HttpClient Initialize(string apiKey)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            return httpClient;

        }

        // Example method to send a request to OpenAI API
        public async Task<string> SendRequestAsync(string prompt)
        {
            // Logic to send a request to OpenAI API and return the response.
            // This is a placeholder implementation.
            await Task.Delay(100); // Simulate async operation
            return $"Response for prompt: {prompt}";
        }

        // Ensure you have the Azure.AI.OpenAI package installed in your project
        // You can install it via NuGet Package Manager or using the following command:
        // dotnet add package Azure.AI.OpenAI

        // Existing code...
        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            try
            {
                var endpoint = "https://hrapppromt.openai.azure.com/openai/deployments/text-embedding-3-small/embeddings?api-version=2023-05-15";
                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var payload = new
                {
                    input = text,
                    model = "text-embedding-3-small"
                };

                var content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await httpClient.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                // Parse the embedding from the response JSON
                using var doc = System.Text.Json.JsonDocument.Parse(responseContent);
                var embeddingArray = doc.RootElement
                    .GetProperty("data")[0]
                    .GetProperty("embedding")
                    .EnumerateArray()
                    .Select(x => x.GetSingle())
                    .ToArray();

                return embeddingArray;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating embedding: {ex.Message}");
                return Array.Empty<float>();
            }
        }

        public async Task<Dictionary<string, float[]>> GetEmbeddingsForLargeTextAsync(string text, int maxChunkSize = 4000)
        {
            var chunks = SplitTextIntoChunks(text, maxChunkSize);
            var chunkEmbeddings = new Dictionary<string, float[]>();
            foreach (var chunk in chunks)
            {
                var embedding = await GetEmbeddingAsync(chunk);
                if (embedding.Length > 0)
                    chunkEmbeddings[chunk] = embedding;
            }
            return chunkEmbeddings;
        }

        private List<string> SplitTextIntoChunks(string text, int maxChunkSize)
        {
            var words = text.Split(' ');
            var chunks = new List<string>();
            var currentChunk = new StringBuilder();
            foreach (var word in words)
            {
                if (currentChunk.Length + word.Length + 1 > maxChunkSize)
                {
                    chunks.Add(currentChunk.ToString());
                    currentChunk.Clear();
                }
                if (currentChunk.Length > 0)
                    currentChunk.Append(' ');
                currentChunk.Append(word);
            }
            if (currentChunk.Length > 0)
                chunks.Add(currentChunk.ToString());
            return chunks;
        }
    }
}
