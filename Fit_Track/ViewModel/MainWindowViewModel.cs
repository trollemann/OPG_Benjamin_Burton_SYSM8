using Fit_Track.Model;
using Fit_Track.View;
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

        //KONSTRUKTOR
        public MainWindowViewModel()
        {
            //tillkalla metod för att initiera användare
            User.InitializeUsers();

            SendKeyCommand = new RelayCommand(ExecuteSendKey);
            SignInCommand = new RelayCommand(ExecuteSignIn, CanExecuteSignIn);
            RegisterCommand = new RelayCommand(ExecuteRegister);
            ForgotPasswordCommand = new RelayCommand(ExecuteForgotPassword);
        }

        //KOMMANDON
        public RelayCommand SendKeyCommand { get; }
        public RelayCommand SignInCommand { get; }
        public RelayCommand RegisterCommand { get; }
        public RelayCommand ForgotPasswordCommand { get; }


        //METODER
        private void ExecuteSendKey(object param)
        {
            Random random = new Random();
            Key = random.Next(1000, 10000);
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

            //hämta användare från listan baserat på användarnamn och lösenord
            var user = User.GetUsers().FirstOrDefault(user => user.Username.ToLower() == Username.ToLower() && 
                                                      user.Password == Password);

            if (Key != Convert.ToInt32(KeyInput))
            {
                MessageBox.Show("Authentication key is wrong, please try again");
                return;
            }

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

        private void ExecuteForgotPassword(object param)
        {
            ForgotPasswordWindow forgotPasswordWindow = new ForgotPasswordWindow();
            var mainWindow = param as Window;
            forgotPasswordWindow.Show();
            Application.Current.Windows[0].Close();
        }
    }
}