using MyFences.Models;

namespace MyFences.ViewModels
{
    public class SetupViewModel : ViewModelBase
    {
        public ApplicationData ApplicationData { get; set; }
        public Fence Fence { get; set; }

        public SetupViewModel(ApplicationData applicationData, Fence fence)
        {
            ApplicationData = applicationData;
            Fence = fence;
        }
    }
}
