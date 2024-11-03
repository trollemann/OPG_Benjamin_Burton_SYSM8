using Fit_Track.Model;
using Fit_Track.View;
using System.Diagnostics.Metrics;
using System.Windows;
using System.Windows.Input;

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

        private string _sendKey;
        public string SendKey
        {
            get => _sendKey;
            set
            {
                _sendKey = value;
                OnPropertyChanged();
            }
        }

        private string _keyInput;
        public string KeyInput
        {
            get => _keyInput;
            set
            {
                _keyInput = value;
                OnPropertyChanged();
            }
        }

        private int _key;
        public int Key
        {
            get => _key;
            set
            {
                _key = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SendKeyCommand { get; }
        public RelayCommand SignInCommand { get; }
        public RelayCommand RegisterCommand { get; }
        public RelayCommand ForgotPasswordCommand { get; }

        //KONSTRUKTOR
        public MainWindowViewModel()
        {
            SendKeyCommand = new RelayCommand(ExecuteSendKey);
            SignInCommand = new RelayCommand(ExecuteSignIn, CanExecuteSignIn);
            RegisterCommand = new RelayCommand(ExecuteRegister);
            ForgotPasswordCommand = new RelayCommand(ExecuteForgotPassword);
            
            //initialisera användare
            User.InitializeUsers();
        }


        //METODER
        private void ExecuteSendKey(object param)
        {
            Random random = new Random();
            Key = random.Next(100000, 1000000);
            MessageBox.Show($"{Key}", "Key", MessageBoxButton.OK);
        }

        private bool CanExecuteSignIn(object param)
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(KeyInput);
        }

        private void ExecuteSignIn(object param)
        {
            var mainWindow = param as Window;

            try
            {
                if (Key != Convert.ToInt32(KeyInput))
                {
                    MessageBox.Show("Authentication key is wrong, please try again");
                    return;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Authentication key is wrong, please try again");
                return;
            }
            
            //kolla efter användare baserat på inloggningsuppgifter
            var user = User.GetUsers().FirstOrDefault(user => user.Username.ToLower() == Username.ToLower() && user.Password == Password);

            if (user != null)
            {
                //sätter den inloggade användaren som nuvarande användare
                User.CurrentUser = user;

                WorkoutsWindow workoutsWindow = new WorkoutsWindow();
                workoutsWindow.Show();
                Application.Current.Windows[0].Close();
            }
            else
            {
                MessageBox.Show("Wrong username or password");
            }
        }

        private void ExecuteRegister(object param)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            Application.Current.Windows[0].Close();

        }

        private void ExecuteForgotPassword(object param)
        {
            ForgotPasswordWindow forgotPasswordWindow = new ForgotPasswordWindow();
            forgotPasswordWindow.Show();
            Application.Current.Windows[0].Close();
        }
    }
}