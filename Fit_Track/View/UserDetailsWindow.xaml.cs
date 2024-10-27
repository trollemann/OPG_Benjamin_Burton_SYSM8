using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Fit_Track.View
{
    /// <summary>
    /// Interaction logic for UserDetailsWindow.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {
        public UserDetailsWindow()
        {
            InitializeComponent();
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