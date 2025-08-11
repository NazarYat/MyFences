using MyFences.Models;
using MyFences.Windows;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace MyFences.ViewModels
{
    public class FenceViewModel : ViewModelBase
    {
        public FenceViewModel() { }
        private readonly string[] _extensionsToHide = new string[] { ".lnk", ".exe" };
        private readonly App _app;
        private readonly Window _window;
        public ApplicationData ApplicationData { get; set; } = null!;
        public Fence Fence { get; set; } = null!;
        public ObservableCollection<ItemViewModel> Items { get; set; } = new ObservableCollection<ItemViewModel>();

        public string Name
        {
            get => Fence.Name;
            set
            {
                if (Fence.Name == value) return;
                Fence.Name = value;
                NotifyOfPropertyChanged();
            }
        }

        private bool _nameEditing = false;
        public bool NameEditing
        {
            get => _nameEditing;
            set
            {
                if (_nameEditing == value) return;
                _nameEditing = value;
                NotifyOfPropertyChanged();
            }
        }

        public FenceViewModel(App app, Fence model, Window window)
        {
            _app = app;
            _window = window;
            Fence = model;
            ApplicationData = app.GetApplicationData();

            CheckItemsExist();
            LoadItems();
        }

        private void CheckItemsExist()
        {
            if (Fence == null || Fence.Items == null)
                Fence = new Fence();

            // Remove invalid items (files or folders)

            for (int i = Fence.Items.Count - 1; i >= 0; i--)
            {
                var item = Fence.Items[i];

                if (!File.Exists(item) && !Directory.Exists(item))
                {
                    Fence.Items.RemoveAt(i);
                }
            }

            _app.SaveData();
        }

        private void LoadItems()
        {
            Items.Clear();

            foreach (var itemPath in Fence.Items)
            {
                var itemVM = CreateItemViewModel(itemPath);
                if (itemVM != null)
                    Items.Add(itemVM);
            }

            NotifyOfPropertyChanged(nameof(Items));
        }

        private ItemViewModel? CreateItemViewModel(string itemPath)
        {
            if (string.IsNullOrEmpty(itemPath) || (!File.Exists(itemPath) && !Directory.Exists(itemPath)))
                return null;


            return new ItemViewModel
            {
                Path = itemPath,
                Name = GetItemName(itemPath),
                Icon = IconHelper.GetHighQualityIcon(itemPath, 256)
            };
        }

        private string GetItemName(string itemPath)
        {
            var isFolder = Directory.Exists(itemPath);

            var t = Path.GetExtension(itemPath).ToLower();

            var r = _extensionsToHide.Contains(t);

            return isFolder ?
                    new DirectoryInfo(itemPath).Name :
                    _extensionsToHide.Contains(Path.GetExtension(itemPath).ToLower()) ?
                        Path.GetFileNameWithoutExtension(itemPath) :
                        Path.GetFileName(itemPath);

        }

        public void AddFile(string path)
        {
            if (!File.Exists(path) && !Directory.Exists(path))
                return;

            if (Fence.Items.Contains(path))
            {
                return;
            }

            Fence.Items.Add(path);

            var newVm = CreateItemViewModel(path);

            if (newVm == null) return;

            Items.Add(newVm);

            NotifyOfPropertyChanged(nameof(Items));

            _app.SaveData();
        }

        public void RemoveFile(ItemViewModel item)
        {
            if (!Fence.Items.Contains(item.Path))
            {
                return;
            }

            Fence.Items.Remove(item.Path);

            Items.Remove(item);

            NotifyOfPropertyChanged(nameof(Items));

            _app.SaveData();
        }
        public void OpenSetupDialog()
        {
            var window = new SetupWindow();

            window.DataContext = new SetupViewModel(
                _app.GetApplicationData(),
                Fence,
                () =>
                {
                    NotifyOfPropertyChanged(nameof(Name));
                    NotifyOfPropertyChanged(nameof(Fence));

                    if (this._window is FenceWindow FV)
                    {
                        FV.UseBlur = Fence.UseBlur;
                    }
                }
            );

            window.ShowDialog();
        }
    }
}
