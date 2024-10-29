using Fit_Track.Model;
using Fit_Track.View;
using System.Windows;

namespace Fit_Track.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        //EGENSKAPER
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        
        //KONSTRUKTOR
        public MainWindowViewModel()
        {
            //tillkalla metod för att initiera användare
            User.InitializeUsers();

            SignInCommand = new RelayCommand(ExecuteSignIn, CanExecuteSignIn);
            RegisterCommand = new RelayCommand(ExecuteRegister);
        }

        //KOMMANDON
        public RelayCommand SignInCommand { get; }
        public RelayCommand RegisterCommand { get; }

        //METODER
        private bool CanExecuteSignIn(object param)
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        private void ExecuteSignIn(object param)
        {
            var mainWindow = param as Window;

            //hämta användare från listan baserat på användarnamn och lösenord
            
            var user = User.GetUsers().FirstOrDefault(user => user.Username == Username && user.Password == Password);
            Username.ToLower();


            //kolla om användarnamn och lösenord är korrekta
            if (user != null)
            {
                //sätt den inloggade användaren som nuvarande användare
                User.CurrentUser = user;

                WorkoutsWindow workoutsWindow = new WorkoutsWindow();
                workoutsWindow.Show();
                mainWindow.Close();
            }
            else
            {
                MessageBox.Show("Wrong username or password");
            }
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