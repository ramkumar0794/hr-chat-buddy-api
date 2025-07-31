using Microsoft.Extensions.Configuration;
using Pinecone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientManager.PineCone
{
    public class PineConeClient : IPineConeClient
    {
        private readonly IConfiguration _config;
        private readonly string _apiKey;
        private readonly string _indexHost;
        private const string DefaultNamespace = "default";
        private const int DefaultTopK = 10;

        public PineConeClient(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _apiKey = _config["pinecone"] ?? throw new InvalidOperationException("Pinecone API key not found in configuration.");
            _indexHost = "hrapp-1kkzlz4.svc.aped-4627-b74a.pinecone.io";
        }

        private IndexClient GetIndexClient()
        {
            var pinecone = new PineconeClient(_apiKey);
            return pinecone.Index(host: _indexHost);
        }

        public async Task<List<string>> GetQueryEmbeddings(float[]? promptEmbedding)
        {
            if (promptEmbedding == null || promptEmbedding.Length == 0)
                return new List<string>();

            var index = GetIndexClient();

            var queryRequest = new QueryRequest
            {
                Vector = promptEmbedding,
                TopK = DefaultTopK,
                Namespace = DefaultNamespace,
                IncludeMetadata = true // Ensure metadata is included in the response
            };

            var queryResponse = await index.QueryAsync(queryRequest);

            if (queryResponse.Matches == null || !queryResponse.Matches.Any())
                return new List<string>();

            return queryResponse.Matches
                .Select(match => match.Metadata?["text"]?.ToString())
                .Where(text => !string.IsNullOrEmpty(text))
                .ToList()!;
        }

        public async Task UpsertEmbeddings(string chunkText, float[] embedding, string id)
        {
            if (string.IsNullOrWhiteSpace(chunkText))
                throw new ArgumentException("Chunk text must not be null or whitespace.", nameof(chunkText));
            if (embedding == null || embedding.Length == 0)
                throw new ArgumentException("Embedding must not be null or empty.", nameof(embedding));
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id must not be null or whitespace.", nameof(id));

            try
            {
                Console.WriteLine($"Pushing chunk: '{chunkText}' (Length: {chunkText.Length})");
                var index = GetIndexClient();

                var vector = new Vector
                {
                    Id = id,
                    Values = embedding,
                    Metadata = new Metadata
                    {
                        ["text"] = new(chunkText)
                    }
                };

                var upsertRequest = new UpsertRequest
                {
                    Vectors = new[] { vector },
                    Namespace = DefaultNamespace
                };

                var upsertResponse = await index.UpsertAsync(upsertRequest);

                // Optionally, check upsertResponse for errors or status
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine($"Error pushing chunk to Pinecone: {ex.Message}");
                throw;
            }
        }
    }
}
