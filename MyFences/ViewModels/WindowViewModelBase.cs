using System.Windows;

namespace MyFences.ViewModels
{
    public abstract class WindowViewModelBase : ViewModelBase
    {
        public readonly ApplicationViewModel _applicationViewModel;
        protected readonly Window _window;
        public WindowViewModelBase(ApplicationViewModel appVm, Window window)
        {
            _applicationViewModel = appVm;
            _window = window;
            _window.Closed += (s, e) =>
            {
                OnWindowClose();
            };
            _window.Loaded += (s, e) =>
            {
                OnWindowLoad();
            };
        }
        public WindowViewModelBase() { _applicationViewModel = new ApplicationViewModel(); }

        protected virtual void OnWindowClose()
        {

        }
        protected virtual void OnWindowLoad()
        {

        }
    }
}
