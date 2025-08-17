using System.Drawing;
using System.IO;
using System.Windows.Media;

namespace MyFences.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        private readonly string[] _extensionsToHide = new string[] { ".lnk", ".exe" };

        public string Name => GetName();
        private string _path = "";
        public string Path 
        {
            get => _path;
            set
            {
                if (value == _path) return;

                _path = value;
                NotifyOfPropertyChanged();
                NotifyOfPropertyChanged(nameof(Name));
            }
        }
        private ImageSource? _icon;
        public ImageSource? Icon 
        {
            get => _icon;
            set
            {
                if (value == _icon) return;

                _icon = value;
                NotifyOfPropertyChanged();
            }
        }

        private string GetName()
        {
            var isFolder = Directory.Exists(Path);

            var t = System.IO.Path.GetExtension(Path).ToLower();

            var r = _extensionsToHide.Contains(t);

            return isFolder ?
                    new DirectoryInfo(Path).Name :
                    _extensionsToHide.Contains(System.IO.Path.GetExtension(Path).ToLower()) ?
                        System.IO.Path.GetFileNameWithoutExtension(Path) :
                        System.IO.Path.GetFileName(Path);

        }
    }
}
