using Fit_Track.ViewModel;
using System.Globalization;
using System.Windows;

namespace Fit_Track.View
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            var viewModel = new RegisterWindowViewModel();
            DataContext = viewModel;
        }
    }
}