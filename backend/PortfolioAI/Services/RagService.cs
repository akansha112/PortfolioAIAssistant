using PortfolioAI.Models;
using PortfolioAI.Repositories;

namespace PortfolioAI.Services
{
    public class RagService
    {
        private readonly IVectorRepository _vectorRepo;
        private readonly IAIService _ai;
        private readonly ChatHistoryRepository _historyRepo;


        public RagService(IVectorRepository vectorRepo, IAIService ai, ChatHistoryRepository historyRepo)
        {
            _vectorRepo = vectorRepo;
            _ai = ai;
            _historyRepo = historyRepo;
        }

        // -------------------------------
        // CHAT QUESTION ANSWERING
        // -------------------------------
        public async Task<string> AskAsync(string question)
        {
            // Step 1: Search similar resume chunks
            var docs = await _vectorRepo.SearchAsync(question);

            if (docs == null || docs.Count == 0)
            {
                return "I don't have information about that yet.";
            }

            // Step 2: Build context
            var context = docs.Count > 0
            ? string.Join("\n", docs.Select((d, i) => $"[{i + 1}] {d.Content}"))
            : "";

            Console.WriteLine("Retrieved Resume Context:");
            Console.WriteLine(context);

            // Step 3: RAG Prompt
            var prompt = $@"
You are an AI assistant for a developer portfolio.

The person described in the context is Akansha Saxena, a SOFTWARE DEVELOPER.

Use ONLY the information provided in the context below to answer the question.

Do NOT use outside knowledge.

If the answer cannot be found in the context, reply:
'I don't have information about that.'

RESUME CONTEXT:
{context}

QUESTION:
{question}

ANSWER:
";

            // Step 4: Ask AI model
            var answer = await _ai.AskAsync(prompt);

            // Save chat history
            var chat = new ChatHistory
            {
                Question = question,
                Answer = answer,
                CreatedAt = DateTime.UtcNow
            };
            await _historyRepo.SaveAsync(chat);

            return answer;
        }


        // ---------------------------------
        // RESUME INDEXING (NEW FEATURE)
        // ---------------------------------
        public async Task IndexResumeAsync(string resumeText)
        {
            if (string.IsNullOrWhiteSpace(resumeText))
                throw new Exception("Resume text is empty");

            // Step 1: Split resume into chunks
            var chunks = SplitIntoChunks(resumeText);

            Console.WriteLine($"Indexing {chunks.Count} resume chunks...");

            // Step 2: Store each chunk in vector DB
            foreach (var chunk in chunks)
            {
                await _vectorRepo.UpsertAsync(chunk);
            }
        }
        public async Task<List<ChatHistory>> GetHistoryAsync()
        {
            return await _historyRepo.GetAllAsync();
        }

        // ---------------------------------
        // HELPER: TEXT CHUNKING
        // ---------------------------------
        private List<string> SplitIntoChunks(string text)
        {
            int chunkSize = 500;

            var chunks = new List<string>();

            for (int i = 0; i < text.Length; i += chunkSize)
            {
                chunks.Add(
                    text.Substring(
                        i,
                        Math.Min(chunkSize, text.Length - i)
                    )
                );
            }

            return chunks;
        }
    }
}