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

        public RelayCommand EnterCommand { get; }
        public RelayCommand ConfirmCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        //KONSTRUKTOR
        public ForgotPasswordWindowViewModel()
        {
            EnterCommand = new RelayCommand(ExecuteEnter, CanExecuteEnter);
            ConfirmCommand = new RelayCommand(ExecuteConfirm, CanExecuteConfirm);
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        //METODER
        private bool CanExecuteEnter(object param)
        {
            return !string.IsNullOrWhiteSpace(Username);
        }

        private void ExecuteEnter(object param)
        {
            //hämtar den första användaren som matchar det angivna användarnamnet
            var user = User.GetUsers().FirstOrDefault(user => user.Username.Equals(Username));

            if (user != null)
            {
                //sätter säkerhetsfrågan till användarens säkerhetsfråga
                SecurityQuestion = user.SecurityQuestion;
                SecurityVisibility = Visibility.Visible;
            }         
        }

        private bool CanExecuteConfirm(object param)
        {
            return !string.IsNullOrWhiteSpace(SecurityAnswer);
        }

        private void ExecuteConfirm(object param)
        {
            //hämtar den första användaren som har det angivna säkerhetssvaret
            var user = User.GetUsers().FirstOrDefault(user => user.SecurityAnswer.Equals(SecurityAnswer));

            //om användaren finns och säkerhetssvaret matchar gör lösenordsfältet synligt
            if (user != null && SecurityAnswer == user.SecurityAnswer)
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
            //kontrollerar att det nya lösenordet matchar bekräftelsen
            if (NewPassword == ConfirmPassword)
            {
                //hämtar den första användaren som matchar det angivna användarnamnet
                var user = User.GetUsers().FirstOrDefault(u => u.Username == Username);

                //sätter användarens lösenord till det bekräftade lösenordet
                user.Password = ConfirmPassword;

                MessageBox.Show("New password has been saved");

                var mainWindow = new MainWindow();
                mainWindow.Show();
                Application.Current.Windows[0].Close();
            }
        }

        private void ExecuteCancel(object param)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.Windows[0].Close();
        }
    }
}