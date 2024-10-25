namespace Fit_Track.Model
{
    public class User : Person
    {
        //statisk lista för o spara användare
        private static List<User> _users = new List<User>();

        //lista för att spara träningspass som är kopplade till denna användare
        public List<Workout> _workouts { get; private set; }

        //properties
        public string Country { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public static User CurrentUser { get; set; }

        //konstructor
        public User(string username, string password, string country, string securityQuestion, string securityAnswer) : base(username, password)
        {
            Country = country;
            SecurityQuestion = securityQuestion;
            SecurityAnswer = securityAnswer;

            //initiera listan för träningspass
            _workouts = new List<Workout>();

            //lägg till ny användare till min lista
            _users.Add(this);
            CurrentUser = this;
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

        public static void InitializeStrengthWorkouts()
        {
            new StrengthWorkout("10/11/24", "Upper body", 60, 0, "intense fullbody workout", 100);
        }

        public static void InitializeCardioWorkouts()
        {
            new CardioWorkout("11/11/24", "Long distance", 60, 0, "run around the park", 10);
        }

        //metod för att få alla mina användare
        public static List<User> GetUsers()
        {
            return _users;
        }

        //overriding metod
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

        //kontrollerar om användare med samma namn redan existerar
        public static bool TakenUsername(string username)
        {
            return _users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
