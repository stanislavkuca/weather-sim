using System.Collections.ObjectModel;
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
using WeatherSim.Models;

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

            weatherItemCollection.ItemsSource = days;
        }

        ObservableCollection<Day> days = new ObservableCollection<Day>();
        Random random = new Random();

        private void SimulateButton(object sender, RoutedEventArgs e)
        {
            days.Clear();

            string tempUnit = " °C";
            int simDayCount = int.Parse(NumDaysTextBox.Text);

            for (int i = 0; i < simDayCount; i++)
            {
                var date = DateTime.Today.AddDays(i);
                var season = GetSeason(date);

                var temperature = GenerateTemperature(season);
                var (weather, icon) = PickWeather(season);

                days.Add(new Day
                {
                    Date = date,
                    ImagePath = icon,
                    Temperature = temperature + tempUnit,
                    Weather = weather
                });
            }
        }

        private string GetSeason(DateTime date)
        {
            int month = date.Month;

            return month switch
            {
                12 or 1 or 2 => "winter",
                3 or 4 or 5 => "spring",
                6 or 7 or 8 => "summer",
                _ => "autumn"
            };
        }

        private int GenerateTemperature(string season)
        {
            return season switch
            {
                "winter" => random.Next(-10, 6),
                "spring" => random.Next(5, 18),
                "summer" => random.Next(18, 36),
                "autumn" => random.Next(5, 15),
                _ => 0
            };
        }

        private (string weather, string iconPath) PickWeather(string season)
        {
            var cloudy = ("Cloudy", "/Assets/Icons/weather_cloudy.png");
            var snowy = ("Snowy", "/Assets/Icons/weather_snowy.png");
            var sunny = ("Sunny", "/Assets/Icons/weather_sunny.png");
            var fog = ("Fog", "/Assets/Icons/weather_fog.png");
            var storm = ("Storm", "/Assets/Icons/weather_storm.png");
            var rain = ("Rain", "/Assets/Icons/weather_rain.png");

            int roll = random.Next(100);

            return season switch
            {
                "winter" => roll switch
                {
                    < 30 => (cloudy),
                    < 70 => (snowy),
                    < 75 => (fog),
                    < 85 => (rain),
                    _ => (sunny)
                },

                "summer" => roll switch
                {
                    < 5 => (fog),
                    < 50 => (sunny),
                    < 60 => (storm),
                    < 90 => (cloudy),
                    _ => (rain),
                },

                _ => (cloudy)
            };
        }
    }
}