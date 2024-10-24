using Fit_Track.Model;
using Fit_Track.View;
using System.Windows;

namespace Fit_Track.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        //egenskaper
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

        //kommandon
        public RelayCommand SignInCommand { get; }
        public RelayCommand RegisterCommand { get; }

        public MainWindowViewModel()
        {
            //tillkalla metod
            User.InitializeUsers();

            SignInCommand = new RelayCommand(ExecuteSignIn, CanExecuteSignIn);
            RegisterCommand = new RelayCommand(ExecuteRegister);
        }

        //kontrollerar om fälten är ifyllda
        private bool CanExecuteSignIn(object param)
        {
            //returnera falskt om användar- och lösenordsfält är tomma
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        private void ExecuteSignIn(object param)
        {
            var mainWindow = param as Window;

            //hämta användare från lista
            var user = User.GetUsers().FirstOrDefault(u => u.Username == Username && u.Password == Password);

            //kolla om användarnamn och lösenord är korrekt
            if (user != null)
            {
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