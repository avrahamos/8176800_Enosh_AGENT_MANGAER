using AgentMvc.Service;
using Microsoft.AspNetCore.Mvc;

namespace AgentMvc.Controllers
{
    public class DashboardController(IDashboardService dashboardService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var viewModel = await dashboardService.GetDashboardDataAsync();
            return View(viewModel);
        }
    }
}
