using System.Collections.ObjectModel;

namespace Fit_Track.Model
{
    public class User : Person
    {
        //håller den för närvarande inloggade användaren
        public static User CurrentUser { get; set; }
        public string Country { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }

        //samling för att hålla användarens träningspass
        public ObservableCollection<Workout> Workouts { get; set; } = new ObservableCollection<Workout>();

        //lista för att spara alla registrerade användare
        public static List<User> _users = new List<User>();

        //lista för att spara träningspass kopplat till användare
        public List<Workout> _workout { get; set; } = new List<Workout>();

        //KONSTRUKTOR
        public User(string username, string password, string country, string securityQuestion, string securityAnswer) : base(username, password)
        {
            Country = country;
            SecurityQuestion = securityQuestion;
            SecurityAnswer = securityAnswer;

            //initierar _workout-listan för användare
            _workout = new List<Workout>();

            //lägger till användarr i _users-listan
            _users.Add(this);
        }

        //initierar existerande användare o träningspass
        public static void InitializeUsers()
        {
            AdminUser.InitializeAdminUser();
            
            if (_users.Count > 2) return;

            var user = new User("user", "password", "Sweden", "What is your favorite exercise?", "Bench press");

            var existingWorkout = new CardioWorkout(DateTime.Parse("01/11/2024"), "Jogging", TimeSpan.FromMinutes(60), 1000, "Morning run", 6);
            var existingWorkout2 = new StrengthWorkout(DateTime.Parse("2024-11-02"), "Upper body", TimeSpan.FromMinutes(120), 300, "Heavy lifting", 200);

            user.AddWorkout(existingWorkout);
            user.AddWorkout(existingWorkout2);
        }

        //hämtar lista med alla användare
        public static List<User> GetUsers()
        {
            return _users;
        }

        //kontrollerar om användarnamn är taget
        public static bool TakenUsername(string username)
        {
            return _users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public void AddWorkout(Workout workout)
        {
            _workout.Add(workout);
        }

        public void RemoveWorkout(Workout workout)
        {
            _workout.Remove(workout);
        }
    }
}