using AgentMvc.Models;
using System.Text.Json;

namespace AgentMvc.Service
{
    public class TargetService(IHttpClientFactory clientFactory) : ITargetService
    {
        private readonly string baseUrl = "https://localhost:7117";
        public async Task<List<TargetModel>> GetAllTaretsAsync()
        {
           var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/Targets");
            var respons = await httpClient.SendAsync(request);
            if (!respons.IsSuccessStatusCode)
            {
                throw new Exception();
            }
            var content = await respons.Content.ReadAsStringAsync();
            List<TargetModel?> targets = JsonSerializer.Deserialize<List<TargetModel?>>(content
                , new JsonSerializerOptions() { PropertyNameCaseInsensitive = true}
                );
            return targets;
        }
    }
}
