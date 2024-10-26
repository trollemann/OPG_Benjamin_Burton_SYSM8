using Fit_Track.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

public static class DataStorage
{
    private const string FilePath = "workouts.json";

    public static void SaveWorkouts(User user)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonData = JsonSerializer.Serialize(user.Workouts, options);
        File.WriteAllText(FilePath, jsonData);
    }

    public static void LoadWorkouts(User user)
    {
        if (File.Exists(FilePath))
        {
            var jsonData = File.ReadAllText(FilePath);
            var workouts = JsonSerializer.Deserialize<ObservableCollection<Workout>>(jsonData);
            if (workouts != null)
            {
                user.Workouts.Clear();
                foreach (var workout in workouts)
                {
                    user.Workouts.Add(workout);
                }
            }
        }
    }
}