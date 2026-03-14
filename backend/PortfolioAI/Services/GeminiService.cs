using Newtonsoft.Json;
using RestSharp;

namespace PortfolioAI.Services
{
    public class GeminiService : IAIService
    {
        private readonly string _apiKey = "AIzaSyDbMONP3TU11QkqHCj8Lu1EjTqs-nkqFeI";

        public async Task<string> AskAsync(string prompt)
        {
            var client = new RestClient(
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}");

            var request = new RestRequest("", Method.Post);

            var body = new
            {
                contents = new[]
                {
                new {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
            };

            request.AddJsonBody(body);

            var response = await client.ExecuteAsync(request);

            dynamic result = JsonConvert.DeserializeObject(response.Content);

            return result.candidates[0].content.parts[0].text;
        }
    }
}