using Fit_Track.Model;
using Fit_Track.View;
using System.Windows;

namespace Fit_Track.ViewModel
{
    public class UserDetailsViewModel : ViewModelBase
    {
        private User _currentUser;
        private bool _isEditable;

        //EGENSKAPER
        public string Username { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }

        public bool IsEditable
        {
            get => _isEditable;
            set
            {
                _isEditable = value;
                OnPropertyChanged();
            }
        }

        //KONSTRUKTOR
        public UserDetailsViewModel(User currentUser)
        {
            //sätter den aktuella användaren
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

        //KOMMANDON
        public RelayCommand EditCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

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

            //uppdaterar egenskaperna i aktuella användare
            _currentUser.Username = Username;
            _currentUser.Password = Password;
            _currentUser.Country = Country;
            _currentUser.SecurityQuestion = SecurityQuestion;
            _currentUser.SecurityAnswer = SecurityAnswer;

            MessageBox.Show("User details have been updated");
            IsEditable = false;
        }

        private void ExecuteCancel(object param)
        {
            if (param is UserDetailsWindow userDetailsWindow)
            {
                userDetailsWindow.Close();
            }
            IsEditable = false;
        }
    }
}