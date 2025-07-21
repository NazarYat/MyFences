using MyFences.Models;
using MyFences.Util;
using System.Collections.ObjectModel;
using System.IO;

namespace MyFences.ViewModels
{
    public class FenceViewModel
    {
        private readonly App _app;
        public Fence Fence { get; set; } = null!;
        public ObservableCollection<ItemViewModel> Items { get; set; } = new ObservableCollection<ItemViewModel>();

        public FenceViewModel(App app, Fence model)
        {
            _app = app;
            Fence = model;

            CheckFiles();
            LoadItems();
        }
        private void CheckFiles()
        {
            if (Fence == null || Fence.Items == null)
                Fence = new Fence();

            foreach (var item in Fence.Items)
            {
                if (!File.Exists(item))
                {
                    Fence.Items.Remove(item);
                }
            }

            _app.SaveData();
        }

        private void LoadItems()
        {
            var items = Fence.Items.Select(i => CreateItemViewModel(i));

            Items = new ObservableCollection<ItemViewModel>(items);
        }
        private ItemViewModel CreateItemViewModel(string itemPath)
        {
            if (string.IsNullOrEmpty(itemPath) || !File.Exists(itemPath))
                return new ItemViewModel();

            var item = new ItemViewModel
            {
                Path = itemPath,
                Name = Path.GetFileName(itemPath),
                Icon = IconHelper.GetIcon(itemPath)
            };
            return item;
        }
    }
}
