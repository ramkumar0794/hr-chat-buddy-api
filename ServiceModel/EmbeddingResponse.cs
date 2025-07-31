namespace ServiceModel
{
    public class EmbeddingResponse
    {
        public EmbeddingData[]? Data { get; set; }
    }
    public class EmbeddingData
    {
        public float[]? Embedding { get; set; }
    }
}
