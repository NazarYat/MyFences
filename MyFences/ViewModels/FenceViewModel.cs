using MyFences.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace MyFences.ViewModels
{
    public class FenceViewModel : ViewModelBase
    {
        private readonly App _app;
        public Fence Fence { get; set; } = null!;
        public ObservableCollection<ItemViewModel> Items { get; set; } = new ObservableCollection<ItemViewModel>();

        public FenceViewModel(App app, Fence model)
        {
            _app = app;
            Fence = model;

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

            var isFolder = Directory.Exists(itemPath);

            return new ItemViewModel
            {
                Path = itemPath,
                Name = isFolder ? new DirectoryInfo(itemPath).Name : Path.GetFileName(itemPath),
                Icon = IconHelper.GetImageSource(itemPath, isFolder)
            };
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

        public void RemoveFile(string path)
        {
            if (!Fence.Items.Contains(path))
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
    }
}
