using Fit_Track.ViewModel;
using System.Windows;

namespace Fit_Track
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new MainWindowViewModel();
            DataContext = viewModel;
            btnSignIn.CommandParameter = this;
        }
    }
}