using MyFences.ViewModels;
using System.Windows;

namespace MyFences.Windows
{
    /// <summary>
    /// Interaction logic for Setup.xaml
    /// </summary>
    public partial class SetupWindow : Window
    {
        public SetupWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SetupViewModel viewModel)
            {
                viewModel.CopyFromSelected();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (DataContext is SetupViewModel viewModel)
            {
                viewModel.CopyOpenedStyleForAll();
            }
        }
    }
}
