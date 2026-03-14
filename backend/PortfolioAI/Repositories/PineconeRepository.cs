using Newtonsoft.Json;
using PortfolioAI.Models;
using RestSharp;

namespace PortfolioAI.Repositories
{
    public class PineconeRepository : IVectorRepository
    {
        private readonly string _apiKey = "pcsk_5H7Xge_6VthomV68zx9a4erp2Ep8Wbp3gnvfXGVSuBmRSLH8C9YidY5HBFgGWjcXoamCaR"; // remove \t
        private readonly string _indexUrl = "https://ai-portfolio-hk303f0.svc.aped-4627-b74a.pinecone.io";
        private readonly string _geminiKey = "AIzaSyDbMONP3TU11QkqHCj8Lu1EjTqs-nkqFeI";
        // -----------------------------
        // SEARCH VECTOR DB
        // -----------------------------
        public async Task<List<DocumentChunk>> SearchAsync(string query)
        {
            Console.WriteLine($"Searching vector DB for query: {query}");

            var embedding = await GenerateEmbedding(query);

            Console.WriteLine($"Embedding size used for search: {embedding.Length}");

            var client = new RestClient(_indexUrl);

            var request = new RestRequest("/query", Method.Post);
            request.AddHeader("Api-Key", _apiKey);
            request.AddHeader("Content-Type", "application/json");

            var body = new
            {
                vector = embedding,
                topK = 5,
                includeMetadata = true
            };

            request.AddJsonBody(body);

            var response = await client.ExecuteAsync(request);

            Console.WriteLine("Pinecone query response:");
            Console.WriteLine(response.Content);

            dynamic result = JsonConvert.DeserializeObject(response.Content);

            var docs = new List<DocumentChunk>();

            if (result?.matches != null)
            {
                foreach (var match in result.matches)
                {
                    docs.Add(new DocumentChunk
                    {
                        Id = match.id,
                        Content = match.metadata.text
                    });
                }
            }

            Console.WriteLine($"Documents retrieved: {docs.Count}");

            return docs;
        }


        // -----------------------------
        // UPSERT DOCUMENT CHUNK
        // -----------------------------
        public async Task UpsertAsync(string content)
        {
            Console.WriteLine("Indexing new resume chunk...");

            var embedding = await GenerateEmbedding(content);

            var client = new RestClient(_indexUrl);

            var request = new RestRequest("/vectors/upsert", Method.Post);

            request.AddHeader("Api-Key", _apiKey);
            request.AddHeader("Content-Type", "application/json");

            var vector = new
            {
                id = Guid.NewGuid().ToString(),
                values = embedding,
                metadata = new
                {
                    text = content
                }
            };

            var body = new
            {
                vectors = new[] { vector }
            };

            request.AddJsonBody(body);

            var response = await client.ExecuteAsync(request);

            Console.WriteLine("Upsert response:");
            Console.WriteLine(response.Content);
            if (!response.IsSuccessful)
                throw new Exception("Pinecone upsert failed: " + response.Content);
        }
        


        // -----------------------------
        // GENERATE EMBEDDING
        // -----------------------------
        private async Task<float[]> GenerateEmbedding(string text)
        {
            Console.WriteLine("Generating embedding...");

            var client = new RestClient(
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-embedding-001:embedContent?key={_geminiKey}");

            var request = new RestRequest("", Method.Post);

            var body = new
            {
                content = new
                {
                    parts = new[]
                    {
                        new { text = text }
                    }
                }
            };

            request.AddJsonBody(body);

            var response = await client.ExecuteAsync(request);

            Console.WriteLine("Embedding API response:");
            Console.WriteLine(response.Content);

            var result = JsonConvert.DeserializeObject<GeminiEmbeddingResponse>(response.Content);

            var fullEmbedding = result.embedding.values;

            Console.WriteLine($"Original embedding dimension: {fullEmbedding.Length}");

            // Pinecone index dimension = 768
            var reducedEmbedding = fullEmbedding.Take(768).ToArray();

            Console.WriteLine($"Reduced embedding dimension: {reducedEmbedding.Length}");

            return reducedEmbedding;
        }
    }
}