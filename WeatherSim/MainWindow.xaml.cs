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
            if (!int.TryParse(NumDaysTextBox.Text, out simDayCount))
            {
                MessageBox.Show("Enter a valid number", "Warning");
                return;
            }

            string selectedSeason = (seasonComboBox.SelectedItem as ComboBoxItem)?.Content.ToString().ToLower() ?? "all";
            DateTime currentDate = DateTime.Today;

            if (selectedSeason != "All")
            {
                while (!IsMonthInSeason(currentDate.Month, selectedSeason))
                    currentDate = currentDate.AddDays(1);
            }
                
            int generatedDays = 0;
            string tempUnit = " °C";

            while (generatedDays < simDayCount)
            {
                string season = generator.GetSeason(currentDate).ToLower();

                if (selectedSeason != "all" && season != selectedSeason)
                {
                    currentDate = currentDate.AddDays(1);
                    continue;
                }

                var (weather, icon) = generator.PickWeather(season);
                int temperature = generator.GenerateTemperature(season);
                
                if (temp_F_Button.IsChecked == true)
                {
                    temperature = generator.CelsiusToFahrenheit(temperature);
                    tempUnit = " °F";
                }

                temps.Add(temperature);

                string displayDate;
                if (selectedSeason == "all")
                {
                    displayDate = generatedDays switch
                    {
                        0 => "Today",
                        1 => "Tomorrow",
                        _ => currentDate.ToString("MMM dd, yyyy"),
                    };
                }
                else
                {
                    displayDate = currentDate.ToString("MMM dd, yyyy");
                }

                days.Add(new Day
                {
                    Date = currentDate,
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

                generatedDays++;
                currentDate = currentDate.AddDays(1);
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

        private bool IsMonthInSeason(int month, string season)
        {
            return season.ToLower() switch
            {
                "winter" => month == 12 || month == 1 || month == 2,
                "spring" => month >= 3 && month <= 5,
                "summer" => month >= 6 && month <= 8,
                "autumn" => month >= 9 && month <= 11,
                _ => true
            };
        }

        private int GetSeasonStartMonth(string season)
        {
            return season.ToLower() switch
            {
                "winter" => 12,
                "spring" => 3,
                "summer" => 6,
                "autumn" => 9,
                _ => 1
            };
        }
    }
}