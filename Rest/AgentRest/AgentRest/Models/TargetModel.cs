namespace AgentRest.Models
{
    public enum IsLiving
    {
        Living= 1,
        Ddead = 2,
    }
    public class TargetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Job {  get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public IsLiving Status { get; set; }



    }
}
