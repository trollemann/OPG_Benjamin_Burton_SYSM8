﻿using Fit_Track.Model;
using Fit_Track.View;
using System.Collections.ObjectModel;
using System.Windows;

namespace Fit_Track.ViewModel
{
    public class RegisterWindowViewModel : ViewModelBase
    {
        public List<string> CountryList { get; } = new List<string> { "Sweden", "Denmark", "Norway", "Iceland" };
        
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
        public RelayCommand CreateNewUserCommand { get; }
        public RelayCommand CancelCommand { get; }
            

        //KONSTRUKTOR
        public RegisterWindowViewModel()
        {
            CreateNewUserCommand = new RelayCommand(ExecuteCreateNewUser, CanExecuteCreateNewUser);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        //METODER
        private bool CanExecuteCreateNewUser(object param)
        {

            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(ConfirmPassword) &&
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

            bool ctrlLength = Length(Password);
            bool ctrlUpperCase = UpperCase(Password);
            bool ctrlSpecialChar = SpecialChar(Password);
            bool ctrlNumber = Number(Password);

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

            if (Password == ConfirmPassword)
            {
                Password = ConfirmPassword;
            }
            else
            {
                MessageBox.Show("Passwords doesn't match");
                return;
            }

            User newUser = new User(Username, Password, Country, SecurityQuestion, SecurityAnswer);
            MessageBox.Show("New user has been created");

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.Windows[0].Close();
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