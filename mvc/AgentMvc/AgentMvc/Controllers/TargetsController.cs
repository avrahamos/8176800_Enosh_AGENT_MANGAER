using AgentMvc.Service;
using Microsoft.AspNetCore.Mvc;

namespace AgentMvc.Controllers
{
    public class TargetsController(ITargetService targetService) : Controller
    {
        public async Task <IActionResult> Index()
        {
            try
            {
                var targets = await targetService.GetAllTaretsAsync();
                return View(targets);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
