using AgentMvc.ViewModel;

namespace AgentMvc.Service
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync();
    }
}
