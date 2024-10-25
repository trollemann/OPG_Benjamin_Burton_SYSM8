using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_Track.Model
{
    public abstract class Person
    {
        //egenskaper
        public string Username { get; private set; }
        public string Password { get; private set; }

        //konstruktor
        public Person(string username, string password)
        {
            Username = username;
            Password = password;
        }

        //abstrakt metod
        public abstract void SignIn();
    }
}