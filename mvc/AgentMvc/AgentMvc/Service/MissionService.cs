using AgentMvc.Models;
using System.Text;
using System.Text.Json;

namespace AgentMvc.Service
{
    public class MissionService(IHttpClientFactory clientFactory) : IMissionService
    {
        private readonly string baseUrl = "https://localhost:7117";
        public async Task<List<MissionModel>> GetAllMissionsAsync()
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/Missions");
            var respons = await httpClient.SendAsync( request );
            if (!respons.IsSuccessStatusCode)
            {
                throw new Exception();
            }
            var content = await respons.Content.ReadAsStringAsync();
            List<MissionModel?> missions = JsonSerializer.Deserialize<List<MissionModel?>>(content ,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true}
                );
            return missions;
        }
        public async Task AssignMissionAsync(int agentId, int targetId)
        {
            var httpClient = clientFactory.CreateClient();
            var response = await httpClient.PostAsync($"{baseUrl}/Missions",
                new StringContent(JsonSerializer.Serialize(new { AgentId = agentId, TargetId = targetId }),
                Encoding.UTF8,
                "application/json"));

            response.EnsureSuccessStatusCode();
        }
    }
}
