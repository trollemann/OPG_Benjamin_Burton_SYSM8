namespace Fit_Track.Model
{
    public class User : Person
    {
        //EGENSKAPER
        public static User CurrentUser { get; set; }
        public string Country { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public bool Admin { get; private set; }

        //lista för att spara användare
        private static List<User> _users = new List<User>();

        //lista för att spara träningspass för varje användare
        public List<Workout> _workout { get; private set; } = new List<Workout>();

        //KONSTRUKTOR
        public User(string username, string password, string country, string securityQuestion, string securityAnswer, bool admin = false) : base(username, password)
        {
            Country = country;
            SecurityQuestion = securityQuestion;
            SecurityAnswer = securityAnswer;
            Admin = admin;
            _workout = new List<Workout>();

            //lägg till ny användare i listan _users
            _users.Add(this);
        }

        //initialisera användare
        public static void InitializeUsers()
        {
            //om det redan finns existerande användare, avbryt metoden
            if (_users.Count > 0) return;

            //skapa existerande användare
            var user = new User("q", "q", "Sweden", "What is your favorite exercise?", "Bench press");
            var admin = new User("admin", "password", "Iceland", "What is your favorite exercise?", "Bicep curls", admin: true);

            //skapa existerande träningspass
            var existingWorkout = new CardioWorkout(DateTime.Parse("2024-11-03"), "Jogging", TimeSpan.FromMinutes(60), 1000, "Morning run", 6);
            var existingWorkout2 = new StrengthWorkout(DateTime.Parse("2024-11-02"), "Upper body", TimeSpan.FromMinutes(120), 300, "Heavy lifting", 200);

            //tilldela user och admin träningspassen
            user.AddWorkout(existingWorkout);
            admin.AddWorkout(existingWorkout2);
        }

        //hämta alla användare
        public static List<User> GetUsers()
        {
            return _users;
        }

        //kontrollerar om användarnamnet är taget
        public static bool TakenUsername(string username)
        {
            return _users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        //lägger till träningspass i användarens lista _workouts
        public void AddWorkout(Workout workout)
        {
            _workout.Add(workout);
        }

        //tar bort träningspass från användarens lista _workouts
        public void RemoveWorkout(Workout workout)
        {
            {
                _workout.Remove(workout);
            }
        }
    }
}