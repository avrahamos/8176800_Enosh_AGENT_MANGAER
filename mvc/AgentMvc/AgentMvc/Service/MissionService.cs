using AgentMvc.Models;
using System.Net.Http;
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
        public async Task<MissionModel?> CreateAndAssignMissionAsync(int agentId, int targetId)
        {
            var httpClient = clientFactory.CreateClient();
            var response = await httpClient.PostAsync($"{baseUrl}/Missions/assign",
                new StringContent(JsonSerializer.Serialize(new { AgentId = agentId, TargetId = targetId }),
                Encoding.UTF8,
                "application/json"));

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<MissionModel>
                (responseContent, new JsonSerializerOptions 
                { PropertyNameCaseInsensitive = true });
        }
      
        public async Task AgentsPursuitAsync()
        {
            var httpClient = clientFactory.CreateClient();
            var response = await httpClient.PostAsync($"{baseUrl}/missions/update", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task<MissionModel?> CommandmentToMissionAsync(int id)
        {
            var httpClient = clientFactory.CreateClient();
            var response = await httpClient.PutAsync($"{baseUrl}/missions/{id}", null);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<MissionModel>
                (content, new JsonSerializerOptions
                { PropertyNameCaseInsensitive = true });
        }
        public async Task<MissionModel?> GetMissionByIdAsync(int id)
        {
            try
            {
                var httpClient = clientFactory.CreateClient();
                var response = await httpClient.GetAsync($"{baseUrl}/missions/GetById /{id}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<MissionModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
                throw;
            }
        }


    }
}
