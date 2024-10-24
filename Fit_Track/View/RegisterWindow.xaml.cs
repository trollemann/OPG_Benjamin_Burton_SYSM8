using Fit_Track.ViewModel;
using System.Globalization;
using System.Windows;

namespace Fit_Track.View
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            var viewModel = new RegisterWindowViewModel();
            DataContext = viewModel;
            btnCreateNewUser.CommandParameter = this;

            // Fyll ComboBox med länder
            comboBoxCountries.ItemsSource = GetAllCountrysNames();
        }

        public static List<string> GetAllCountrysNames()
        {
            // Hämta alla specifika kulturer
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            return cultures
                .Select(static cult =>
                {
                    // Försök att skapa RegionInfo, fånga undantaget för anpassade kulturer
                    try
                    {
                        // Använd kulturs namn för att få den engelska namnet på landet
                        RegionInfo region = new RegionInfo(cult.Name);
                        return region.EnglishName; // Returnera namnet på landet på engelska
                    }
                    catch
                    {
                        return null; // Returnera null för anpassade kulturer
                    }
                })
                .Where(name => name != null) // Filtrera bort null-värden
                .Distinct() // Se till att vi har unika namn
                .OrderBy(q => q) // Sortera alfabetiskt
                .ToList();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }
    }
}