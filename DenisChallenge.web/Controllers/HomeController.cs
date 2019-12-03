using DenisChallenge.Service.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DenisChallenge.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAanbodApi _aanbodApi;
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config, IAanbodApi aanbodApi)
        {
            _config = config;
            _aanbodApi = aanbodApi;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MakelaarsLijst()
        {
            bool isTuin = false;
            var groeperingsTabelViewModel = _aanbodApi.GetTopMakelaars(_config, isTuin);

            return View(groeperingsTabelViewModel);
        }

        public IActionResult MakelaarsLijstTuin()
        {
            bool isTuin = true;
            var groeperingsTabelViewModel = _aanbodApi.GetTopMakelaars(_config, isTuin);

            return View(groeperingsTabelViewModel);
        }
    }
}

