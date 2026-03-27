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
            for (int i = 0; i < 10; i++)
            {
                days.Add(new Day
                {
                    Date = DateTime.Now.AddDays(i),
                    ImagePath = "/Assets/Icons/weather_cloudy.png",
                    Temperature = "15" + tempUnit,
                    Weather = "Sunny"
                });
            }

            weatherItemCollection.ItemsSource = days;
        }
    }
    public class Day
    {
        public DateTime Date { get; set; }
        public string? ImagePath { get; set; }
        public string? Temperature { get; set; }
        public string? Weather {  get; set; }
    }
}