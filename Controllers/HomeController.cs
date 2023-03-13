using ConditionTable.Abstracts;
using ConditionTable.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace ConditionTable.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRuleService _ruleService;

        public HomeController(ILogger<HomeController> logger, IRuleService ruleService)
        {
            _logger = logger;
            _ruleService = ruleService;
        }
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("GetInterval/{value}")]
        public IActionResult GetInterval(decimal value)
        {
            var result = _ruleService.GetRuleByIntervalValue(value);
            return PartialView("_ResultPartialView", result);
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