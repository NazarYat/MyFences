namespace MyFences.Models
{
    public class Fence
    {
        public string Name { get; set; } = "New Fence";
        public float Width { get; set; } = 100;
        public float Height { get; set; } = 100;
        public float Left { get; set; } = 100;
        public float Top { get; set; } = 100;
        public List<string> Items { get; set; } = new List<string>();
    }
}
