using System.Collections.ObjectModel;
using System.Reflection.Emit;
using System.Text;
using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        List<int> temps = new List<int>();
        Dictionary<string, int> weatherCounts = new Dictionary<string, int>();
        WeatherGenerator generator = new WeatherGenerator();
        
        private void SimulateButton(object sender, RoutedEventArgs e)
        {
            days.Clear();
            temps.Clear();
            weatherCounts.Clear();

            int simDayCount = 0;
            string tempUnit = " °C";

            try
            {
                simDayCount = int.Parse(NumDaysTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Enter a valid number", "Warning");
            }

            for (int i = 0; i < simDayCount; i++)
            {
                var date = DateTime.Today.AddDays(i);
                var season = generator.GetSeason(date);
                var (weather, icon) = generator.PickWeather(season);
                var temperature = generator.GenerateTemperature(season);
                
                if (temp_F_Button.IsChecked == true)
                {
                    temperature = generator.CelsiusToFahrenheit(temperature);
                    tempUnit = " °F";
                }

                string displayDate = i switch
                {
                    0 => "Today",
                    1 => "Tomorrow",
                    _ => date.ToString("MMM dd, yyyy"),
                };

                temps.Add(temperature);

                days.Add(new Day
                {
                    Date = date,
                    DisplayDate = displayDate,
                    ImagePath = icon,
                    Temperature = temperature + tempUnit,
                    Weather = weather
                });

                // Weather Count
                if (weatherCounts.ContainsKey(weather))
                    weatherCounts[weather]++;
                else
                    weatherCounts[weather] = 1;
            }

            // Avg Temp Calc
            double avgTemp = Math.Round(temps.Average(), 1);
            avgTempTextBlock.Text = avgTemp.ToString() + tempUnit;

            // Max Temp
            int maxTemp = temps.Max();
            maxTempTextBlock.Text = maxTemp.ToString() + tempUnit;

            // Min Temp
            int minTemp = temps.Min();
            minTempTextBlock.Text = minTemp.ToString() + tempUnit;

            // Weather Output
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in weatherCounts)
            {
                sb.AppendLine($"{kvp.Key}: {kvp.Value}");
            }

            weatherCountTextBlock.Text = sb.ToString();
        }
    }
}