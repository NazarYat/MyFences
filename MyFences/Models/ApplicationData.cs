using System.Windows;

namespace MyFences.Models
{
    public class ApplicationData
    {
        // Grid

        public Thickness GridMargin { get; set; } = new Thickness(0, 0, 0, 0);
        public int GridColumns { get; set; } = 20;
        public int GridRows { get; set; } = 20;
        public bool UseGrid { get; set; } = true;

        // Fences
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
                ItemIconSize = 32,
                ItemHeight = 80,
                ItemWidth = 80,
                Items = new List<string>
                {
                    "C:\\Users\\yatsy\\OneDrive\\Рабочий стол\\SoundPanel.lnk"
                }
            });
        }
    }
}
