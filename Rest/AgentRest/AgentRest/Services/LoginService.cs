namespace AgentRest.Services
{
    public class LoginService(IJwtService jwtService) : ILoginService
    {
        static List<string> AuthoriseServers = ["SimulationServer", "MvcServer"];
        public string Login(string serverName)
        {
            if (AuthoriseServers.Contains(serverName))
            {
                return jwtService.CreateToken(serverName);
            }
            throw new Exception("this server is unauthorise"); ;
        }
    }
}
