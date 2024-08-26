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
        
        [HttpPost]
        public async Task<IActionResult> UpdateMissions()
        {
            await missionService.AgentsPursuitAsync();
            return RedirectToAction("Index");
        }
       
        public async Task<IActionResult> Assign(int id)
        {
            var mission = await missionService.GetMissionByIdAsync(id);
            return View(mission);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmAssign(int id)
        {
            try
            {
                await missionService.CommandmentToMissionAsync(id);
                return RedirectToAction("Index");
            }
            catch 
            {
                return View("Eror");
            }
        }

    }
}
