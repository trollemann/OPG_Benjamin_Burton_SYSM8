using Fit_Track.Model;
using Fit_Track.View;
using System.Windows;

namespace Fit_Track.ViewModel
{
    public class UserDetailsViewModel : ViewModelBase
    {
        private User _currentUser;
        private bool _edit;

        //EGENSKAPER
        public string Username { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }

        public bool Edit
        {
            get => _edit;
            set
            {
                _edit = value;
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

            Edit = false;
        }

        //KOMMANDON
        public RelayCommand EditCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        //METODER
        private void ExecuteEdit(object param)
        {
            Edit = true;
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

            //uppdaterar egenskaperna i aktuella användare
            _currentUser.Username = Username;
            _currentUser.Password = Password;
            _currentUser.Country = Country;
            _currentUser.SecurityQuestion = SecurityQuestion;
            _currentUser.SecurityAnswer = SecurityAnswer;

            MessageBox.Show("User details have been updated");
            Edit = false;
        }

        private void ExecuteCancel(object param)
        {
            if (param is UserDetailsWindow userDetailsWindow)
            {
                userDetailsWindow.Close();
            }
            Edit = false;
        }
    }
}