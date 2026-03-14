using Microsoft.AspNetCore.Mvc;
using PortfolioAI.Services;

namespace PortfolioAI.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly RagService _ragService;

        public AdminController(RagService ragService)
        {
            _ragService = ragService;
        }

        [HttpPost("resume")]
        public async Task<IActionResult> UploadResume(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Resume file missing");

            // Step 1: Extract text from PDF
            var resumeText = PdfHelper.ExtractText(file);

            if (string.IsNullOrWhiteSpace(resumeText))
                return BadRequest("Could not extract text from PDF");

            // Step 2: Index resume in RAG
            await _ragService.IndexResumeAsync(resumeText);

            return Ok(new { message = "Resume indexed successfully" });
        }
    }
}