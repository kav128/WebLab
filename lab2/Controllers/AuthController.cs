using System.Threading.Tasks;
using lab2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace lab2.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IVkApiService _vkApiService;

        public AuthController(ILogger<AuthController> logger, IVkApiService vkApiService)
        {
            _logger = logger;
            _vkApiService = vkApiService;
        }

        // GET
        public async Task<IActionResult> Index(string? code, string? error, [FromQuery(Name = "error_description")] string? errorDescription)
        {
            if (error is not null)
            {
                _logger.LogWarning($"Got error '{error}' with description '{errorDescription}'");
                return Redirect("/");
            }

            if (code is null)
            {
                _logger.LogError("Authentication code is null");
                return BadRequest();
            }
            
            _logger.LogInformation("Got auth code");
            
            await _vkApiService.Auth(code);
            
            return Redirect("/");
        }
    }
}
