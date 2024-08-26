using AgentMvc.Service;
using Microsoft.AspNetCore.Mvc;

namespace AgentMvc.Controllers
{
    public class MissionController(IMissionService  missionService) : Controller
    {
        public async Task< IActionResult> Index()
        {
            var missions =await missionService.GetAllMissionsAsync();
            return View(missions);
        }
    }
}
