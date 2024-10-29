using System.Collections.ObjectModel;

namespace Fit_Track.Model
{
    public class User : Person
    {
        //statisk lista för o spara användare
        private static List<User> _users = new List<User>();

        //lista för att spara träningspass 
        public List<Workout> Workouts { get; private set; } = new List<Workout>();

        //EGENSKAPER        public List<Workout> Workouts { get; private set; } = new List<Workout>();
        public static User CurrentUser { get; set; }
        public string Country { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public bool Admin { get; private set; }

        //KONSTRUKTOR
        public User(string username, string password, string country, string securityQuestion, string securityAnswer, bool admin = false) : base(username, password)
        {
            Country = country;
            SecurityQuestion = securityQuestion;
            SecurityAnswer = securityAnswer;
            Admin = admin;
            Workouts = new List<Workout>();

            //lägg till ny användare
            _users.Add(this);
        }

        //METODER
        //lägg till träningspass
        public void AddWorkout(Workout workout)
        {
            Workouts.Add(workout);
        }

        public void RemoveWorkout(Workout workout)
        {
            if (workout != null && Workouts.Contains(workout))
            {
                Workouts.Remove(workout);
            }
        }

        //initialisera användare o undvika dubletter
        public static void InitializeUsers()
        {
            if (_users.Count == 0)
            {
                var user = new User("user", "password", "Sweden", "What is your favorite exercise?", "Bench press");
                var user2 = new User("user2", "password", "Denmark", "What is your favorite exercise?", "Squats");
                var admin = new User("admin", "password", "Somalia", "What is your favorite exercise?", "Bicep curls", admin: true);

                // Lägg till ett existerande träningspass för användaren "user"
                var existingWorkout = new CardioWorkout("2024-11-03", "Jogging", 60, 1000, "Morning run", 60);
                var existingWorkout2 = new StrengthWorkout("2024-11-02", "Upper body", 120, 300, "Heavy lifting", 200);
                var existingWorkout3 = new StrengthWorkout("2024-11-04", "Lower Body", 60, 300, "Endurance", 200);

                user.AddWorkout(existingWorkout);
                user.AddWorkout(existingWorkout2);
                admin.AddWorkout(existingWorkout3);
            }
        }

        //hämta alla användare
        public static List<User> GetUsers()
        {
            return _users;
        }

        // Override metod för inloggning
        public override void SignIn()
        {
            Console.WriteLine($"{Username} has signed in");
        }

        //lösenordsåterställning
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

        //kontrollerar om användarnamnet är taget
        public static bool TakenUsername(string username)
        {
            return _users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}