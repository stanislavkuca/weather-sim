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
using WeatherSim.Helpers;
using WeatherSim.Models;
using WeatherSim.Services;

namespace WeatherSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<Day> days = new ObservableCollection<Day>();
        private readonly List<int> temps = new List<int>();
        private Dictionary<string, int> weatherCounts = new Dictionary<string, int>();
        private readonly WeatherGenerator generator = new();

        public MainWindow()
        {
            InitializeComponent();
            weatherItemCollection.ItemsSource = days;
            InitializeWeatherCounts();
        }

        private void InitializeWeatherCounts()
        {
            weatherCounts = WeatherKeys.All.ToDictionary(k => k, v => 0);
        }

        private void SimulateButton(object sender, RoutedEventArgs e)
        {
            ResetCollections();

            if (!int.TryParse(NumDaysTextBox.Text, out int simDayCount))
            {
                MessageBox.Show("Enter a valid number", "Warning");
                return;
            }

            string selectedSeason = GetSelectedSeason();
            DateTime currentDate = SeasonHelper.AlignStartDateToSeason(DateTime.Today, selectedSeason);
 
            int generatedDays = 0;
            string tempUnit = " °C";

            while (generatedDays < simDayCount)
            {
                string season = generator.GetSeason(currentDate).ToLower();

                if (!IsDateInSelectedSeason(season, selectedSeason))
                {
                    currentDate = currentDate.AddDays(1);
                    continue;
                }

                int temperature = GenerateTemperature(season, ref tempUnit);
                temps.Add(temperature);

                string displayDate = DisplayDateService.GetDisplayDate(selectedSeason, generatedDays, currentDate);

                AddDayEntry(currentDate, displayDate, temperature, tempUnit, season);

                generatedDays++;
                currentDate = currentDate.AddDays(1);
            }

            UpdateStatistics(tempUnit);
        }

        // ----------------------------
        // Helpers
        // ----------------------------

        private void ResetCollections()
        {
            days.Clear();
            temps.Clear();
            InitializeWeatherCounts();
        }

        private string GetSelectedSeason()
        {
            return (seasonComboBox.SelectedItem as ComboBoxItem)?
                .Content.ToString()?.ToLower() ?? "all";
        }

        private bool IsDateInSelectedSeason(string season, string selectedSeason)
        {
            return selectedSeason == "all" || season == selectedSeason;
        }

        private int GenerateTemperature(string season, ref string tempUnit)
        {
            int temp = generator.GenerateTemperature(season);

            if (temp_F_Button.IsChecked == true)
            {
                temp = generator.CelsiusToFahrenheit(temp);
                tempUnit = " °F";
            }

            return temp;
        }

        private void AddDayEntry(DateTime date, string displayDate, int temperature, string tempUnit, string season)
        {
            var (weather, icon) = generator.PickWeather(season);

            weather = weather.Trim().ToLower();

            days.Add(new Day
            {
                Date = date,
                DisplayDate = displayDate,
                ImagePath = icon,
                Temperature = temperature + tempUnit,
                Weather = weather
            });

            if (weatherCounts.ContainsKey(weather))
                weatherCounts[weather]++;
            else
                weatherCounts[weather] = 1;
        }

        private void UpdateStatistics(string tempUnit)
        {
            avgTempTextBlock.Text = Math.Round(temps.Average(), 1) + tempUnit;
            maxTempTextBlock.Text = temps.Max() + tempUnit;
            minTempTextBlock.Text = temps.Min() + tempUnit;

            weatherCountTextBlock.Text = StatisticsService.BuildWeatherCountText(weatherCounts);
        }
    }
}