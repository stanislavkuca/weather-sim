using System.Collections.ObjectModel;
using System.Reflection.Emit;
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
using WeatherSim.Services;

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
        WeatherGenerator generator = new WeatherGenerator();

        private void SimulateButton(object sender, RoutedEventArgs e)
        {
            days.Clear();

            string tempUnit = " °C";
            int simDayCount = int.Parse(NumDaysTextBox.Text);

            for (int i = 0; i < simDayCount; i++)
            {
                var date = DateTime.Today.AddDays(i);
                var season = generator.GetSeason(date);

                var temperature = generator.GenerateTemperature(season);
                var (weather, icon) = generator.PickWeather(season);

                days.Add(new Day
                {
                    Date = date,
                    ImagePath = icon,
                    Temperature = temperature + tempUnit,
                    Weather = weather
                });
            }
        }
    }
}