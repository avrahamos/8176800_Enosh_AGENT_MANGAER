namespace AgentRest.Models
{
    public enum IsActive
    {
        Active = 1,
        NoActive = 2,
    }
    public class AgentModel
    {
        public  int Id { get; set; }
        public string NicName { get; set; }
        public string Image { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public IsActive Status { get; set; }


    }
}
