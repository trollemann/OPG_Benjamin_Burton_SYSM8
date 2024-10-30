using Fit_Track.Model;
using System.Windows;

namespace Fit_Track.ViewModel
{
    internal class ForgotPasswordWindowViewModel : ViewModelBase
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

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged();
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
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

        private Visibility _securityVisibility = Visibility.Collapsed;
        public Visibility SecurityVisibility
        {
            get => _securityVisibility;
            set
            {
                _securityVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _passwordVisibility = Visibility.Collapsed;
        public Visibility PasswordVisibility
        {
            get => _passwordVisibility;
            set
            {
                _passwordVisibility = value;
                OnPropertyChanged();
            }
        }

        //KONSTRUKTOR
        public ForgotPasswordWindowViewModel()
        {
            EnterCommand = new RelayCommand(ExecuteEnter, CanExecuteEnter);
            ConfirmCommand = new RelayCommand(ExecuteConfirm, CanExecuteConfirm);
            SaveCommand = new RelayCommand(ExecuteEnter, CanExecuteEnter);
        }

        //KOMMANDON
        public RelayCommand EnterCommand { get; }
        public RelayCommand ConfirmCommand { get; }
        public RelayCommand SaveCommand { get; }

        //METODER
        private bool CanExecuteEnter(object param)
        {
            return !string.IsNullOrWhiteSpace(Username);
        }

        private void ExecuteEnter(object param)
        {
            var user = User.GetUsers().FirstOrDefault(user => user.Username.Equals(Username));
            
            if (user != null)
            {
                SecurityQuestion = user.SecurityQuestion;
                SecurityVisibility = Visibility.Visible;
            }
            else
            {
                SecurityQuestion = string.Empty;
                SecurityVisibility = Visibility.Collapsed;
            }
        }

        private bool CanExecuteConfirm(object param)
        {
            return !string.IsNullOrWhiteSpace(SecurityAnswer);
        }
        private void ExecuteConfirm(object param)
        {
            var user = User.GetUsers().FirstOrDefault(user => user.SecurityAnswer.Equals(SecurityAnswer));
            if (SecurityAnswer == user.SecurityAnswer)
            {
                PasswordVisibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Wrong security answer");
            }
        }

        private bool CanExecuteSave(object param)
        {
            return !string.IsNullOrWhiteSpace(NewPassword) &&
                   !string.IsNullOrWhiteSpace(ConfirmPassword);
        }

        private void ExecuteSave(object param)
        {
            if (NewPassword == ConfirmPassword)
            {
                Password = NewPassword;

                var forgotPasswordWindow = param as Window;
                var mainWindow = new MainWindow();
                mainWindow.Show();
                forgotPasswordWindow.Close();
            }
        }
    }
}