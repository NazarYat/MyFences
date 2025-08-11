using MyFences.Models;
using MyFences.Util;
using MyFences.ViewModels;
using MyFences.Windows;
using System.Windows;

namespace MyFences;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private ApplicationData _appData = new();
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _appData = SerializationUtil.LoadFromFile<ApplicationData>("data.json") ?? new TestApplicationData();

        foreach (var fence in _appData.Fences)
        {
            var window = new FenceWindow();

            var viewModel = new FenceViewModel(this, fence, window);

            window.DataContext = viewModel;

            window.Show();
        }

        string[] args = Environment.GetCommandLineArgs();

        if (args.Length > 1)
        {
            string param = args[1]; // Should be "true"
            if (bool.TryParse(param, out bool flag) && flag)
            {
                // Create window
            }
        }
    }
    public ApplicationData GetApplicationData()
    {
        return _appData;
    }
    public void SaveData()
    {
        //SerializationUtil.SaveToFile("data.json", _appData);
    }
}
