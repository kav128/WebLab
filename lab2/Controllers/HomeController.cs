using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using lab2.Models;
using lab2.Services;
using Microsoft.AspNetCore.Authentication;

namespace lab2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVkApiService _vkApiService;

        public HomeController(ILogger<HomeController> logger, IVkApiService vkApiService)
        {
            _logger = logger;
            _vkApiService = vkApiService;
        }

        public IActionResult Index()
        {
            ViewData.Add("AuthUrl", _vkApiService.GetAuthorizeUrl());
            return View(_vkApiService.CurrentUser);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
