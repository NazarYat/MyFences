using MyFences.ViewModels;
using System.IO.Pipes;
using System.Text;
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

        bool createNewFence = false;

        if (e.Args.Length > 0)
        {
            foreach (string arg in e.Args)
            {
                if (arg == "NewFence") createNewFence = true;
            }
        }

        if (createNewFence)
        {
            if (TryConnectToServerAndCreateNewFence())
            {
                Environment.Exit(0);
                return;
            }
        }

        _applicationViewModel = new ApplicationViewModel();

        _applicationViewModel.Start();

        if (createNewFence)
        {
            _applicationViewModel.CreateNewFence();
        }
    }

    private bool TryConnectToServerAndCreateNewFence()
    {
        try
        {
            using (var client = new NamedPipeClientStream(".", "MyFences", PipeDirection.Out))
            {
                client.Connect(200);

                SendMessage(client, "NewFence");

                return true;
            }
        }
        catch
        {
            return false;
        }
    }
    static void SendMessage(NamedPipeClientStream client, string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        client.Write(buffer, 0, buffer.Length);
        client.Flush();
    }

    override protected void OnExit(ExitEventArgs e)
    {
        _applicationViewModel?.SaveData();
        base.OnExit(e);
    }
}
