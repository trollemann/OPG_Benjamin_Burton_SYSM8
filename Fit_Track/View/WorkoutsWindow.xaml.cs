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
            var viewModel = new WorkoutsWindowViewModel();
            DataContext = viewModel;
            
            btnUserDetails.CommandParameter = this;
            btnAddWorkout.CommandParameter = this;
            btnRemoveWorkout.CommandParameter = this;
            btnWorkoutDetails.CommandParameter = this;
        }
    }
}