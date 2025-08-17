using MyFences.Models;
using MyFences.Util;
using MyFences.Windows;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Windows;

namespace MyFences.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        public ApplicationData AppData { get; private set; } = new();
        public Dictionary<Fence, FenceViewModel> FenceViewModels { get; private set; } = new Dictionary<Fence, FenceViewModel>();

        public string settingsFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyFences", "data.json");

        public void Start()
        {
            AppData = SerializationUtil.LoadFromFile<ApplicationData>(settingsFilePath) ?? new ApplicationData();

            foreach (var fence in AppData.Fences)
            {
                CreateFenceWindow(fence);
            }

            Task.Run(StartServer);
        }
        private async Task StartServer()
        {
            while (true)
            {
                try
                {
                    using (var server = new NamedPipeServerStream("MyFences", PipeDirection.InOut))
                    {
                        await server.WaitForConnectionAsync();

                        var buffer = new byte[256];
                        int bytesRead;

                        while ((bytesRead = server.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                            if (message == "NewFence")
                            {
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    CreateNewFence();
                                });
                            }
                        }
                    }
                }
                catch { }
            }
        }

        private readonly object _lock = new();
        private Task? _task = null;
        private bool _needSaveData = false;
        private CancellationTokenSource? _cts = null;

        public void SaveData()
        {
            lock (_lock)
            {
                _needSaveData = true;

                if (_task != null && !_task.IsCompleted)
                    return;

                _cts = new CancellationTokenSource();
                _task = Task.Run(() => SaveDataInternalAsync(_cts.Token));
            }
        }

        private async Task SaveDataInternalAsync(CancellationToken token)
        {
            while (true)
            {
                bool saveNow;
                lock (_lock)
                {
                    saveNow = _needSaveData;
                    _needSaveData = false;
                }

                if (!saveNow || token.IsCancellationRequested)
                    break;

                try
                {
                    await Task.Delay(3000, token);
                    if (token.IsCancellationRequested) break;

                    await SerializationUtil.SaveToFileAsync(settingsFilePath, AppData);
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(
                            $"Failed to save data: {ex.Message}",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                    });

                    return;
                }
            }
        }

        public void CreateNewFence()
        {
            var fence = new Fence();

            if (AppData.Fences.Any()) fence.CopyStyleFrom(AppData.Fences.Last());

            AppData.Fences.Add(fence);

            CreateFenceWindow(fence);

            SaveData();
        }
        public void DeleteFence(Fence fence)
        {
            AppData.Fences.Remove(fence);

            FenceViewModels.Remove(fence);

            SaveData();
        }
        public FenceWindow CreateFenceWindow(Fence fence)
        {
            var window = new FenceWindow();

            var viewModel = new FenceViewModel(this, fence, window);

            FenceViewModels.Add(fence, viewModel);

            window.DataContext = viewModel;

            window.Show();

            return window;
        }
    }
}
