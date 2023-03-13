using Microsoft.AspNetCore.Mvc;

namespace ConditionTable.Controllers
{
    public class ConditionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
