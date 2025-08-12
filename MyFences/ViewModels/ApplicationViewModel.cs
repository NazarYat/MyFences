using MyFences.Models;
using MyFences.Util;
using MyFences.Windows;

namespace MyFences.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        public ApplicationData AppData { get; private set; } = new();
        public Dictionary<Fence, FenceViewModel> FenceViewModels { get; private set; } = new Dictionary<Fence, FenceViewModel>();

        public void Start()
        {
            AppData = SerializationUtil.LoadFromFile<ApplicationData>("data.json") ?? new TestApplicationData();

            foreach (var fence in AppData.Fences)
            {
                CreateFenceWindow(fence);
            }
        }

        public void SaveData()
        {
            //SerializationUtil.SaveToFile("data.json", _appData);
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
