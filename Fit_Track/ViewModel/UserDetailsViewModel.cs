using Fit_Track.Model;
using Fit_Track.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_Track.ViewModel
{
    public class UserDetailsViewModel : ViewModelBase
    {
        // Properties for displaying user details
        public string Username { get; }
        public string Password { get; }
        
        private string _country;
        public string Country
        {
            get { return _country; }
            set
            {
                _country = value;
                OnPropertyChanged();
            }
        }
        public string SecurityQuestion { get; }
        public string SecurityAnswer { get; }

        public RelayCommand Edit { get; }
        public RelayCommand Save { get; }
        public RelayCommand Cancel { get; }

        // Constructor that accepts the current user
        public UserDetailsViewModel(User currentUser)
        {
            Username = currentUser.Username;
            Password = currentUser.Password;
            Country = currentUser.Country;
            SecurityQuestion = currentUser.SecurityQuestion;
            SecurityAnswer = currentUser.SecurityAnswer;

            Edit = new RelayCommand(ExecuteEdit);
            Save = new RelayCommand(ExecuteSave);
            Cancel = new RelayCommand(ExecuteCancel);
        }
        private void ExecuteEdit(object param)
        {

        }

        private void ExecuteSave(object param)
        {

        }

        private void ExecuteCancel(object param)
        {
            if (param is UserDetailsWindow userDetailsWindow)
            {
                userDetailsWindow.Close();
            }
        }
    }
}