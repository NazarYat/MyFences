using MyFences.ViewModels;
using System.Windows;

namespace MyFences;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private ApplicationViewModel _applicationViewModel = null!;
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _applicationViewModel = new ApplicationViewModel();

        _applicationViewModel.Start();
    }
}
