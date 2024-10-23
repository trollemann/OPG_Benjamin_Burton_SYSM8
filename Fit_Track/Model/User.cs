using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_Track.Model
{
    public class User : Person
    {
        public string Country { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public User(string username, string password, string country, string securityQuestion, string securityAnswer) : base(username, password)
        {
            Country = country;
            SecurityQuestion = securityQuestion;
            SecurityAnswer = securityAnswer;
        }

        public override void SignIn()
        {
            Console.WriteLine($"{Username} has signed in");
        }

        public void ResetPassword(string securityAnswer)
        {
            if (SecurityAnswer == SecurityAnswer)
            {
                Console.WriteLine("password has been resetted");
            }
            else
            {
                Console.WriteLine("security answer is incorrect");
            }
        }
    }
}
