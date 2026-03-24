using System.Text;
using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeatherSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string tempUnit = " °C";

            List<Day> days = new List<Day>();
            days.Add(new Day() { Date = "24.03.2026", ImagePath = "path-placeholder", Temperature = "26.1" + tempUnit, Weather = "Sunny"});
            days.Add(new Day() { Date = "25.03.2026", ImagePath = "path-placeholder", Temperature = "14.9" + tempUnit, Weather = "Stormy" });
            days.Add(new Day() { Date = "26.03.2026", ImagePath = "path-placeholder", Temperature = "10" + tempUnit, Weather = "Cloudy" });

            weatherItemCollection.ItemsSource = days;
        }
    }
    public class Day
    {
        public string? Date { get; set; }
        public string? ImagePath { get; set; }
        public string? Temperature { get; set; }
        public string? Weather {  get; set; }
    }
}