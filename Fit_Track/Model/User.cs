using System.Collections.ObjectModel;

namespace Fit_Track.Model
{
    public class User : Person
    {
        //statisk lista for o spara användare
        private static List<User> _users = new List<User>();

        //observerbara samlingar för träningspass
        public ObservableCollection<Workout> Workouts { get; private set; } = new ObservableCollection<Workout>();
        public static User CurrentUser { get; set; }

        //egenskaper
        public string Country { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }

        //konstruktor
        public User(string username, string password, string country, string securityQuestion, string securityAnswer)
            : base(username, password)
        {
            Country = country;
            SecurityQuestion = securityQuestion;
            SecurityAnswer = securityAnswer;

            //lägg till ny användare
            _users.Add(this);
        }

        //metod för o lägga till träningspass
        public void AddWorkout(Workout workout)
        {
            Workouts.Add(workout);
        }

        //metod för o initialisera användare o undvika dubletter
        public static void InitializeUsers()
        {
            if (_users.Count == 0)
            {
                new User("user", "password", "Sweden", "What is your favorite exercise?", "Bench press");
            }
        }

        //metod för o hämta alla användare
        public static List<User> GetUsers()
        {
            return _users;
        }

        // Override metod för inloggning
        public override void SignIn()
        {
            Console.WriteLine($"{Username} has signed in");
        }

        //metod för lösenordsåterställning
        public void ResetPassword(string securityAnswer)
        {
            if (SecurityAnswer == securityAnswer)
            {
                Console.WriteLine("Password has been reset");
            }
            else
            {
                Console.WriteLine("Security answer is incorrect");
            }
        }

        //metod för o se om användarnamnet är taget
        public static bool TakenUsername(string username)
        {
            return _users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}