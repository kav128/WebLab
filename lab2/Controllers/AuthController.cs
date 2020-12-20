using System;
using System.Threading.Tasks;
using lab2.Entities;
using lab2.Models;
using lab2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace lab2.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IVkAuthService _vkAuthService;
        private readonly IVkApiService _vkApiService;

        public AuthController(ILogger<AuthController> logger, IVkAuthService vkAuthService, IVkApiService vkApiService)
        {
            _logger = logger;
            _vkAuthService = vkAuthService;
            _vkApiService = vkApiService;
        }

        public IActionResult Vk()
        {
            string url = _vkAuthService.AuthorizeUrl;
            _logger.LogInformation($"Redirecting to {url}");
            return Redirect(url);
        }

        public async Task<IActionResult> Index(string? code, string? error, [FromQuery(Name = "error_description")] string? errorDescription)
        {
            if (error is not null || errorDescription is not null)
            {
                _logger.LogWarning($"Got error '{error}' with description '{errorDescription}'");
                var model = new VkErrorModel { ExceptionType = error!, Message = errorDescription! };
                return View(model);
            }

            if (code is null)
            {
                _logger.LogError("Authentication code is null");
                var model = new VkErrorModel { ExceptionType = "Bad query", Message = "Authentication code is null" };
                return View(model);
            }

            _logger.LogInformation("Got auth code");

            try
            {
                AccessTokenData accessToken = await _vkAuthService.Auth(code);
                HttpContext.Session.Set("AccessToken", accessToken);
                
                User user = await _vkApiService.GetUser(accessToken.UserId, accessToken.AccessToken);
                HttpContext.Session.Set("User", user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to authorize in VK");
                var errorModel = new VkErrorModel { ExceptionType = e.GetType().Name, Message = e.Message };
                return View(errorModel);
            }

            return Redirect("/");
        }
    }
}
