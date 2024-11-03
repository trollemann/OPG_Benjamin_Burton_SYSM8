using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_Track.Model
{
    public class AdminUser : User
    {
        public AdminUser(string username, string password, string country, string securityQuestion, string securityAnswer)
            : base(username, password, country, securityQuestion, securityAnswer)
        {
        }

        public static new void InitializeAdminUser()
        {
            var admin = new AdminUser("admin", "password", "Iceland", "What is your favorite exercise?", "Bicep curls");
            User._users.Add(admin);
        }
    }
}