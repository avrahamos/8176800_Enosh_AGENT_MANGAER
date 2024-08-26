namespace AgentRest.Services
{
    public interface IJwtService
    {
        string CreateToken(string name);
    }
}
