using System.Drawing;
using System.Windows.Media;

namespace MyFences.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
        public ImageSource? Icon { get; set; }
    }
}
