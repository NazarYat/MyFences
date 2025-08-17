using MyFences.Models;
using MyFences.Windows;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace MyFences.ViewModels
{
    public class FenceViewModel : WindowViewModelBase
    {
        public FenceViewModel() : base() { }

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

        public FenceViewModel(ApplicationViewModel appVM, Fence model, Window window) : base(appVM, window)
        {
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

            _applicationViewModel.SaveData();
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
            
            StartTrackingFiles();

            NotifyOfPropertyChanged(nameof(Items));
        }

        private ItemViewModel? CreateItemViewModel(string itemPath)
        {
            if (string.IsNullOrEmpty(itemPath) || (!File.Exists(itemPath) && !Directory.Exists(itemPath)))
                return null;


            return new ItemViewModel
            {
                Path = itemPath,
                Icon = IconHelper.GetHighQualityIcon(itemPath, 256)
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

            StartTrackingFile(path);

            NotifyOfPropertyChanged(nameof(Items));

            _applicationViewModel.SaveData();
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

            _applicationViewModel.SaveData();
        }
        public void OpenSetupDialog()
        {
            var window = new SetupWindow();

            window.DataContext = new SetupViewModel(
                _applicationViewModel,
                window,
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

        public void CreateNewFence()
        {
            _applicationViewModel.CreateNewFence();
        }

        public void DeleteFence()
        {
            _window.Close();
            _applicationViewModel.DeleteFence(Fence);
        }

        private readonly Dictionary<string, FileSystemWatcher> Watchers = new Dictionary<string, FileSystemWatcher>(); // folder / watcher

        private void StartTrackingFiles()
        {
            foreach (var file in Items)
            {
                StartTrackingFile(file.Path);
            }
        }
        private void StartTrackingFile(string path)
        {
            var directory = Path.GetDirectoryName(path);
            
            if (directory == null) return;

            if (Watchers.ContainsKey(directory)) return;

            StartTrackingDirectory(directory);
        }
        private void StartTrackingDirectory(string path)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.Filter = "*.*";
            watcher.IncludeSubdirectories = false;

            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            
            watcher.EnableRaisingEvents = true;

            Watchers.Add(path, watcher);
        }
        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var item = Items.FirstOrDefault(i => i.Path == e.FullPath);

                if (item == null) return;

                RemoveFile(item);
            });
        }
        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var index = Fence.Items.IndexOf(e.OldFullPath);

                if (index < 0) return;

                Fence.Items[index] = e.FullPath;

                var item = Items.FirstOrDefault(i => i.Path == e.OldFullPath);

                if (item == null) return;

                item.Path = e.FullPath;

                NotifyOfPropertyChanged(nameof(Items));
                _applicationViewModel.SaveData();
            });
        }
    }
}
