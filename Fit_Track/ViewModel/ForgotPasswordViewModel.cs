using Fit_Track.Model;
using Fit_Track.View;
using System.Windows;

namespace Fit_Track.ViewModel
{
    public class ForgotPasswordViewModel : ViewModelBase
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

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        public User CurrentUser { get; }
        public RelayCommand SaveCommand { get; }




        //constructor


        //method
        private void ExecuteSave(object param)
        {
            if (Password == ConfirmPassword)
            {
                if (CurrentUser != null)
                {
                    CurrentUser.Password = Password;
                    MessageBox.Show("Password has been changed");
                    
                    PasswordWindow passwordWindow = new PasswordWindow();
                    var mainWindow = param as Window;
                    mainWindow.Show();
                    passwordWindow.Close();

                }
            }
            else
            {
                MessageBox.Show("Passwords doesn't match");
            }
        }
    }
}
