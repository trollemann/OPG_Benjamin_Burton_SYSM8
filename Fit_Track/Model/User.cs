namespace Fit_Track.Model
{
    public class User : Person
    {
        //statisk lista för o spara användare
        private static List<User> _users = new List<User>();

        //properties
        public string Country { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }

        //constructor
        public User(string username, string password, string country, string securityQuestion, string securityAnswer) : base(username, password)
        {
            Country = country;
            SecurityQuestion = securityQuestion;
            SecurityAnswer = securityAnswer;

            //lägg till ny användare till min lista
            _users.Add(this);
        }

        //hämtar användarinformation
        public static void InitializeUsers()
        {
            //kolla om listan är tom för att undvika duplikanter
            if (_users.Count == 0)
            {
                new User("user", "password", "Sweden", "What is your favorite exercise?", "Bench press");
            }
        }

        //metod för att få alla mina användare
        public static List<User> GetUsers()
        {
            return _users;
        }

        //overriding method
        public override void SignIn()
        {

            Console.WriteLine($"{Username} has signed in");
        }

        //method
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
