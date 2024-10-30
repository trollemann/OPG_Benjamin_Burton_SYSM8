using Fit_Track.Model;
using Fit_Track.ViewModel;
using System.Windows;

namespace Fit_Track.View
{
    /// <summary>
    /// Interaction logic for WorkoutsWindow.xaml
    /// </summary>
    public partial class WorkoutsWindow : Window
    {
        public WorkoutsWindow()
        {
            InitializeComponent();
            var viewModel = new WorkoutsWindowViewModel(User.CurrentUser);
            DataContext = viewModel;
        }
    }
}