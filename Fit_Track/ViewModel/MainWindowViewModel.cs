using Fit_Track.Model;
using Fit_Track.View;
using System.Windows;

namespace Fit_Track.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
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

        public RelayCommand SignInCommand { get; }
        public RelayCommand RegisterCommand { get; }

        //användarlista
        private List<User> _users;
        public MainWindowViewModel()
        {
            //lista med existerande användare
            _users = new List<User> { new User("user", "password", "Sweden", "What is your favorite exercise?", "Bench press") };

            SignInCommand = new RelayCommand(ExecuteSignIn, CanExecuteSignIn);
            RegisterCommand = new RelayCommand(ExecuteRegister, CanExecuteRegister);

        }

        //kontrollerar om btnSign kan användas
        private bool CanExecuteSignIn(object param)
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        private void ExecuteSignIn(object param)
        {
            //type casta en obj param till ett window-obj
            //param lagras i variabeln mainWindow
            //kan nu tillkalla Windows-metoder
            var mainWindow = param as Window;

            //hämtar första elementet i listan
            var user = _users.FirstOrDefault(u => u.Username == Username && u.Password == Password);

            //kontrollerar om användarnamn och lösenord är korrekt
            if (user != null)
            {
                WorkoutsWindow workoutsWindow = new WorkoutsWindow();
                workoutsWindow.Show();
                mainWindow.Close();
            }
            else
            {
                //om ej korrekt öppnas MessageBox
                MessageBox.Show("Felaktigt användarnamn eller lösenord");
            }
        }

        private bool CanExecuteRegister(object param)
        {
            return true;
        }

        private void ExecuteRegister(object param)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            var mainWindow = param as Window;
            registerWindow.Show();
            mainWindow.Close();
        }
    }
}