using AgentMvc.Models;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace AgentMvc.Service
{
    public class AgentService(IHttpClientFactory clientFactory) : IAgentService
    {
        private readonly string baseUrl = "https://localhost:7117";
        public async Task<List<AgentModel?>> GetAllAgentsAsync()
        {
            var httpClient =clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get,$"{baseUrl}/Agents");
            var respons = await httpClient.SendAsync(request);
            if (!respons.IsSuccessStatusCode) 
            {
                throw new Exception();
            }
           
            var content = await respons.Content.ReadAsStringAsync();
            List<AgentModel?>agents = JsonSerializer.Deserialize<List<AgentModel?>>(content ,
                new JsonSerializerOptions() {PropertyNameCaseInsensitive= true });
            return agents;
        }
    }
}
