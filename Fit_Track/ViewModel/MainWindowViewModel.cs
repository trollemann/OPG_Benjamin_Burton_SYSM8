using Fit_Track.Model;
using Fit_Track.View;
using System.Windows;
using System.Windows.Input;

namespace Fit_Track.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        //användarlista
        private List<User> _users;

        //properties
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        //kopplar RelayCommand så man kan binda till btnSignIn
        public RelayCommand SignInCommand { get; }

        public MainWindowViewModel()
        {
            //lista med existerande användare
            _users = new List<User>
        {
            new User("user", "password", "Sweden", "What is your favorite exercise?", "Bench press")
        };

            SignInCommand = new RelayCommand(ExecuteSignIn, CanExecuteSignIn);
        }

        //kontrollerar om btnSign kan användas
        private bool CanExecuteSignIn(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        private void ExecuteSignIn(object parameter)
        {
            //metod från LINQ, hämtar första elementet i listan
            //'u' representerar enskilt objekt i listan _user
            var user = _users.FirstOrDefault(u => u.Username == Username && u.Password == Password);

            //kontrollerar om användarnamn och lösenord är korrekt
            //om de är korrekt, öppna WorkoutsWindow
            if (user != null)
            {
                WorkoutsWindow workoutsWindow = new WorkoutsWindow();
                workoutsWindow.Show();
            }

            //om ej korrekt öppnas messageBox
            else
            {
                MessageBox.Show("felaktigt användarnamn eller lösenord");
            }
        }
    }
}