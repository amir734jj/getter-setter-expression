namespace core.Tests.Models
{
    public class Human
    {
        public string Name { get; set; }
        
        public int Age { get; set; }
        
        public Human GrandParent { get; set; }
    }
}