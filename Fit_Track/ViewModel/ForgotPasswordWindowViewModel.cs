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
            bool ctrlLength = Length(NewPassword);
            bool ctrlUpperCase = UpperCase(NewPassword);
            bool ctrlSpecialChar = SpecialChar(NewPassword);
            bool ctrlNumber = Number(NewPassword);


            if (!ctrlLength && !ctrlUpperCase && !ctrlSpecialChar)
            {
                MessageBox.Show("The password must contain at least eight characters, one uppercase letter, and one special character");
                return;
            }
            else if (!ctrlLength && !ctrlUpperCase)
            {
                MessageBox.Show("The password must contain at least eight characters and at least one uppercase letter");
                return;
            }
            else if (!ctrlLength && !ctrlSpecialChar)
            {
                MessageBox.Show("The password must contain at least eight characters and at least one special character");
                return;
            }
            else if (!ctrlUpperCase && !ctrlSpecialChar)
            {
                MessageBox.Show("The password must contain at least one uppercase letter and one special character");
                return;
            }
            else if (!ctrlLength)
            {
                MessageBox.Show("The password must be at least eight characters long");
                return;
            }
            else if (!ctrlUpperCase)
            {
                MessageBox.Show("The password must contain at least one uppercase letter");
                return;
            }
            else if (!ctrlSpecialChar)
            {
                MessageBox.Show("The password must contain at least one special character");
                return;
            }
            else if (!ctrlNumber)
            {
                MessageBox.Show("The password must cointain at least one number");
                return;
            }

            if (Password != ConfirmPassword)
            {
                Password = ConfirmPassword;
            }
            else
            {
                MessageBox.Show("Passwords doesn't match");
                return;
            }

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

        static bool Length(string password)
        {
            return password.Length >= 8;
        }

        static bool UpperCase(string password)
        {
            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    return true;
                }
            }
            return false;
        }

        static bool SpecialChar(string password)
        {
            foreach (char c in password)
            {
                if (char.IsSymbol(c) || char.IsPunctuation(c))
                {
                    return true;
                }
            }
            return false;
        }
        static bool Number(string password)
        {
            foreach (char c in password)
            {
                if (char.IsNumber(c))
                {
                    return true;
                }
            }
            return false;
        }
    }
}