using Fit_Track.Model;
using Fit_Track.View;
using System.Windows;

namespace Fit_Track.ViewModel
{
    public class RegisterWindowViewModel : ViewModelBase
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
        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                _country = value;
                OnPropertyChanged();
            }
        }
        private string _securityQuestion;
        public string SecurityQuestion
        {
            get => _securityQuestion;
            set
            {
                _securityQuestion = value;
                OnPropertyChanged();
            }
        }
        private string _securityAnswer;
        public string SecurityAnswer
        {
            get => _securityAnswer;
            set
            {
                _securityAnswer = value;
                OnPropertyChanged();
            }
        }

        //KONSTRUKTOR
        public RegisterWindowViewModel()
        {
            CreateNewUserCommand = new RelayCommand(ExecuteCreateNewUser, CanExecuteCreateNewUser);
        }

        //KOMMANDON
        public RelayCommand CreateNewUserCommand { get; }

        //METODER
        private bool CanExecuteCreateNewUser(object param)
        {

            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(Country) &&
                   !string.IsNullOrWhiteSpace(SecurityQuestion) &&
                   !string.IsNullOrWhiteSpace(SecurityAnswer);
        }

        private void ExecuteCreateNewUser(object param)
        {
            var registerWindow = param as Window;

            if (User.TakenUsername(Username))
            {
                MessageBox.Show("Username already taken");
                return;
            }

            User newUser = new User(Username, Password, Country, SecurityQuestion, SecurityAnswer);
            MessageBox.Show("New user has been created");

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            registerWindow.Close();
        }
    }
}