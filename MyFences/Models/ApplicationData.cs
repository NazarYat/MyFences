namespace MyFences.Models
{
    public class ApplicationData
    {
        public List<Fence> Fences { get; set; } = new List<Fence>();
    }

    public class TestApplicationData : ApplicationData
    {
        public TestApplicationData()
        {
            Fences.Add(new Fence
            {
                Name = "Test Fence",
                Width = 200,
                Height = 150,
                Left = 50,
                Top = 50,
                Items = new List<string>
                {

                }
            });
        }
    }
}
