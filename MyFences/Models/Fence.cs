using System.Windows.Media;
using MyFences.Util;

namespace MyFences.Models
{
    public class Fence
    {
        public List<string> Items { get; set; } = new List<string>();

        // Common
        public string Name { get; set; } = "New Fence";
        public float Width { get; set; } = 100;
        public float Height { get; set; } = 100;
        public float Left { get; set; } = 100;
        public float Top { get; set; } = 100;
        public bool UseBlur { get; set; } = true;
        public int BorderThickness { get; set; } = 0;

        // Window
        public Color BackgroundColor { get; set; } = ColorHelper.FromString("#330059FF");
        public Color WindowBorderColor { get; set; } = ColorHelper.FromString("#00000000");
        public Color HeaderColor { get; set; } = ColorHelper.FromString("#490059FF");
        public int HeaderHeight { get; set; } = 20;

        // Items
        public int Spacing { get; set; } = 5;
        public int ItemHeight { get; set; } = 65;
        public int ItemWidth { get; set; } = 65;
        public int ItemIconSize { get; set; } = 25;
        public Color ItemColor { get; set; } = ColorHelper.FromString("#00000000");
        public Color ItemBorderColor { get; set; } = ColorHelper.FromString("#00000000");
        public int ItemBorderThickness { get; set; } = 0;
        public Color HighlightedItemColor { get; set; } = ColorHelper.FromString("#22FFFFFF");
        public Color HighlightedItemBorderColor { get; set; } = ColorHelper.FromString("#00000000");
        public int HighlightedItemBorderThickness { get; set; } = 0;
        public Color SelectedItemColor { get; set; } = ColorHelper.FromString("#44FFFFFF");
        public Color SelectedItemBorderColor { get; set; } = ColorHelper.FromString("#00000000");
        public int SelectedItemBorderThickness { get; set; } = 0;

        // Text
        public Color ItemTextColor { get; set; } = ColorHelper.FromString("#FFFFFFFF");
        public Color HeaderTextColor { get; set; } = ColorHelper.FromString("#FFFFFFFF");
        public int ItemFontSize { get; set; } = 12;
        public int HeaderFontSize { get; set; } = 12;

        // Scroll bar
        public Color ScrollBarColor { get; set; } = ColorHelper.FromString("#FF81A9FF");
        public Color ScrollBarBorderColor { get; set; } = ColorHelper.FromString("#00000000");
        public int ScrollBarBorderThickness { get; set; } = 0;
        public int ScrollBarCornerRadius { get; set; } = 3;
        public int ScrollBarWidth { get; set; } = 5;
    }
}
