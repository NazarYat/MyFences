namespace MyFences.ViewModels
{
    public abstract class WindowViewModelBase : ViewModelBase
    {
        public readonly ApplicationViewModel _applicationViewModel;
        public WindowViewModelBase(ApplicationViewModel appVm)
        {
            _applicationViewModel = appVm;
        }
        public WindowViewModelBase() { _applicationViewModel = new ApplicationViewModel(); }
    }
}
