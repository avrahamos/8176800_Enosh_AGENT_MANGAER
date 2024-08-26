using AgentMvc.Service;
using Microsoft.AspNetCore.Mvc;

namespace AgentMvc.Controllers
{
    public class AgentsController(IAgentService agentService) : Controller
    {
        public async Task <IActionResult> Index()
        {
            try
            {
                var agents = await agentService.GetAllAgentsAsync();
                return View(agents);
            }
            catch
            {
                return RedirectToAction("Index","Home");
            }
        }
    }
}
