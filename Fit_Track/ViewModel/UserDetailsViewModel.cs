﻿using Fit_Track.Model;
using Fit_Track.View;
using System.Windows;

namespace Fit_Track.ViewModel
{
    public class UserDetailsViewModel : ViewModelBase
    {
        private User _currentUser;
        public List<string> CountryList { get; } = new List<string> { "Sweden", "Norway", "Denmark", "Iceland" };

        //EGENSKAPER
        private bool _isEditable;
        public bool IsEditable
        {
            get => _isEditable;
            set
            {
                _isEditable = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PasswordVisibility));
                OnPropertyChanged(nameof(NewPasswordVisibility));
                OnPropertyChanged(nameof(ConfirmPasswordVisibility));
            }
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Country { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }

        public Visibility PasswordVisibility => IsEditable ? Visibility.Collapsed : Visibility.Visible;
        public Visibility NewPasswordVisibility => IsEditable ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ConfirmPasswordVisibility => IsEditable ? Visibility.Visible : Visibility.Collapsed;

        //KOMMANDON
        public RelayCommand EditCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        //KONSTRUKTOR
        public UserDetailsViewModel(User currentUser)
        {
            _currentUser = currentUser;

            Username = currentUser.Username;
            Password = currentUser.Password;
            Country = currentUser.Country;
            SecurityQuestion = currentUser.SecurityQuestion;
            SecurityAnswer = currentUser.SecurityAnswer;

            EditCommand = new RelayCommand(ExecuteEdit);
            SaveCommand = new RelayCommand(ExecuteSave);
            CancelCommand = new RelayCommand(ExecuteCancel);

            IsEditable = false;
        }

        //METODER
        private void ExecuteEdit(object param)
        {
            IsEditable = true;
        }

        private void ExecuteSave(object param)
        {
            if (User.TakenUsername(Username) && Username != _currentUser.Username)
            {
                MessageBox.Show("Username already taken");
                return;
            }

            if (Username.Length < 3)
            {
                MessageBox.Show("Username must be at least 3 characters");
                return;
            }

            if (NewPassword == ConfirmPassword)
            {
                Password = NewPassword;
            }
            else
            {
                MessageBox.Show("Passwords doesn't match");
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(NewPassword))
                {
                    MessageBox.Show("Please input new password");
                    return;
                }

                if (NewPassword.Length < 5)
                {
                    MessageBox.Show("Password must be at least 5 characters");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                return;
            }

            if (string.IsNullOrEmpty(SecurityQuestion))
            {
                MessageBox.Show("Please enter a security question");
                return;
            }

            if (string.IsNullOrEmpty(SecurityAnswer))
            {
                MessageBox.Show("Please enter a security answer");
                return;
            }

            //uppdaterar egenskaperna för den nuvarande användaren
            _currentUser.Username = Username;
            _currentUser.Password = Password;
            _currentUser.Country = Country;
            _currentUser.SecurityQuestion = SecurityQuestion;
            _currentUser.SecurityAnswer = SecurityAnswer;

            MessageBox.Show("User details has been updated");

            var workoutsWindow = new WorkoutsWindow();
            workoutsWindow.Show();
            Application.Current.Windows[0].Close();

        }

        private void ExecuteCancel(object param)
        {
            var workoutsWindow = new WorkoutsWindow();
            workoutsWindow.Show();
            Application.Current.Windows[0].Close();
        }
    }
}