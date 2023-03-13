using ConditionTable.Abstracts;
using ConditionTable.Entity;
using ConditionTable.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Linq;

namespace ConditionTable.Controllers
{
    [Route("Condition")]
    public class ConditionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRuleService _ruleService;

        public ConditionController(ILogger<HomeController> logger, IRuleService ruleService)
        {
            _logger = logger;
            _ruleService = ruleService;
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var result = _ruleService.GetAllRules();
            return View(result);
        }
        [HttpGet("Rules")]
        public IActionResult Rules()
        {
            var result = _ruleService.GetAllRules();
            return View(result);
        }

        [HttpPost("AddRule")]
        public IActionResult AddRule([FromBody] RuleDbModel model)
        {

            var result = _ruleService.AddRule(model);
            return PartialView("_RuleTable", result);
        }
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(string id)
        {
            var rule = _ruleService.GetAllRules().Rules.Where(x => x.Id == id).FirstOrDefault();
            return PartialView("_EditRule", rule);
        }

        [HttpPost("EditRule")]
        public IActionResult EditRule(Models.Rule rule) 
        {
            var result = _ruleService.EditRule(rule);
            return PartialView("_EditRule");
        }
        [HttpGet("Delete/{id}")]
        public IActionResult Delete(string id)
        {
            _ruleService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
