using Fit_Track.Model;
using Fit_Track.View;
using System.Windows;

namespace Fit_Track.ViewModel
{
    public class RegisterWindowViewModel : ViewModelBase
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

        private string _country;
        public string Country
        {
            get { return _country; }
            set
            {
                _country = value;
                OnPropertyChanged();
            }
        }

        private string _securityQuestion;
        public string SecurityQuestion
        {
            get { return _securityQuestion; }
            set
            {
                _securityQuestion = value;
                OnPropertyChanged();
            }
        }

        private string _securityAnswer;
        public string SecurityAnswer
        {
            get { return _securityAnswer; }
            set
            {
                _securityAnswer = value;
                OnPropertyChanged();
            }
        }

        //kommandon
        public RelayCommand CreateNewUserCommand { get; }

        //konstruktor
        public RegisterWindowViewModel()
        {
            CreateNewUserCommand = new RelayCommand(ExecuteCreateNewUser, CanExecuteCreateNewUser);
        }

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