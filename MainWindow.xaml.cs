using Newtonsoft.Json;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeatherApi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		static int day = 0;
		static string[] locationList = 
			{ "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/Kyiv?unitGroup=metric&key=JXR6MRQ576KHZT8JQYLRDLWM4&contentType=json",
			"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/Odessa?unitGroup=metric&key=JXR6MRQ576KHZT8JQYLRDLWM4&contentType=json",
			"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/Ternopil?unitGroup=metric&key=JXR6MRQ576KHZT8JQYLRDLWM4&contentType=json",
			"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/Paris?unitGroup=metric&key=JXR6MRQ576KHZT8JQYLRDLWM4&contentType=json",
			"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/Warsaw?unitGroup=metric&key=JXR6MRQ576KHZT8JQYLRDLWM4&contentType=json",
			"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/Krakow?unitGroup=metric&key=JXR6MRQ576KHZT8JQYLRDLWM4&contentType=json",
			"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/Berlin?unitGroup=metric&key=JXR6MRQ576KHZT8JQYLRDLWM4&contentType=json"
		};
		static int locationCounter = 0;

        public MainWindow()
        {
            InitializeComponent();
		}

		private string ToWords(int number)
		{
			string[] words = {
			"Zero", "One", "Two", "Three", "Four", "Five", "Six",
			"Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", 
			"Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen",
			"Twenty", "TwentyOne", "TwentyTwo", "TwentyThree"
			};

			return words[number];
		}


		public async Task GetInfo()
		{
			var client = new HttpClient();

			var request = new HttpRequestMessage(HttpMethod.Get,
			locationList[locationCounter]);

			var response = await client.SendAsync(request);
			response.EnsureSuccessStatusCode();

			var body = await response.Content.ReadAsStringAsync();

			dynamic weather = JsonConvert.DeserializeObject(body);

			string icon = weather.currentConditions.icon;

			Dispatcher.Invoke(() =>
			{
				switch (locationCounter)
				{
					case 0:
						LocationText.Text = "Kyiv";
						LocationText.FontSize = 48;
						LocationText.Margin = new Thickness(132, 32, 70, 58);
						break;
					case 1:
						LocationText.Text = "Odessa";
						LocationText.FontSize = 44;
						LocationText.Margin = new Thickness(110, 37, 70, 58);
						break;
					case 2:
						LocationText.Text = "Ternopil";
						LocationText.FontSize = 40;
						LocationText.Margin = new Thickness(110, 38, 70, 58);
						break;
					case 3:
						LocationText.Text = "Paris";
						LocationText.FontSize = 44;
						LocationText.Margin = new Thickness(132, 36, 70, 58);
						break;
					case 4:
						LocationText.Text = "Warsaw";
						LocationText.FontSize = 42;
						LocationText.Margin = new Thickness(110, 38, 70, 58);
						break;
					case 5:
						LocationText.Text = "Krakow";
						LocationText.FontSize = 44;
						LocationText.Margin = new Thickness(110, 38, 70, 58);
						break;
					case 6:
						LocationText.Text = "Berlin";
						LocationText.FontSize = 44;
						LocationText.Margin = new Thickness(120, 38, 70, 58);
						break;
				}

				DateText.Text = weather.days[day].datetime;
				WeatherDescriptionText.Text = weather.days[day].description;
				HigherTemperatureText.Text = $"H:{Convert.ToInt32(weather.days[day].tempmax)}";
				LowerTemperatureText.Text = $"L:{Convert.ToInt32(weather.days[day].tempmin)}";
				TemperatureText.Text = $"{Convert.ToInt32(weather.currentConditions.temp)}°";

				for (int i = 0; i <= 23; i++) 
				{
					var hourData = weather.days[day].hours[i];
					string icon = hourData.icon;
					string iconPath;

					switch (icon)
					{
						case "sunny":
							iconPath = "sunny.png";
							break;
						case "rain":
							iconPath = "rain.png";
							break;
						case "partly-cloudy-night":
							iconPath = "partly-cloudy-night.png";
							break;
						case "partly-cloudy":
							iconPath = "partly-cloudy.png";
							break;
						case "clear-night":
							iconPath = "clear-night.png";
							break;
						case "snow":
							iconPath = "blowing-snow.png";
							break;
						case "blizzard":
							iconPath = "blizzard.png";
							break;
						case "cloudy":
						default:
							iconPath = "cloudy.png";
							break;
					}

					var image1 = FindName($"{ToWords(i)}HoursImage") as Image;
					if (image1 != null)
					{
						image1.Source = new BitmapImage(new Uri(iconPath, UriKind.Relative));
					}
				}


				var image = new BitmapImage();

				switch (icon)
				{
					case "cloudy":
						image = new BitmapImage(new Uri("cloudyWeather.png", UriKind.Relative));
						BackgroundBrush.ImageSource = image;
						WeatherRectangle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1B2735"));

						MyLocation.Foreground = Brushes.White;
						LocationText.Foreground = Brushes.White;
						TemperatureText.Foreground = Brushes.White;
						WeatherText.Foreground = Brushes.White;
						HigherTemperatureText.Foreground = Brushes.White;
						LowerTemperatureText.Foreground = Brushes.White;
						WeatherDescriptionText.Foreground = Brushes.White;
						TodaysDate.Foreground = Brushes.White;
						DateText.Foreground = Brushes.White;

						ZeroHoursText.Foreground = Brushes.White;
						OneHoursText.Foreground = Brushes.White;
						TwoHoursText.Foreground = Brushes.White;
						ThreeHoursText.Foreground = Brushes.White;
						FourHoursText.Foreground = Brushes.White;
						FiveHoursText.Foreground = Brushes.White;
						SixHoursText.Foreground = Brushes.White;
						SevenHoursText.Foreground = Brushes.White;
						EightHoursText.Foreground = Brushes.White;
						NineHoursText.Foreground = Brushes.White;
						TenHoursText.Foreground = Brushes.White;
						ElevenHoursText.Foreground = Brushes.White;
						TwelveHoursText.Foreground = Brushes.White;
						ThirteenHoursText.Foreground = Brushes.White;

						FourteenHoursText.Foreground = Brushes.White;
						FifteenHoursText.Foreground = Brushes.White;
						SixteenHoursText.Foreground = Brushes.White;
						SeventeenHoursText.Foreground = Brushes.White;
						EighteenHoursText.Foreground = Brushes.White;
						NineteenHoursText.Foreground = Brushes.White;
						TwentyHoursText.Foreground = Brushes.White;
						TwentyOneHoursText.Foreground = Brushes.White;
						TwentyTwoHoursText.Foreground = Brushes.White;
						TwentyThreeHoursText.Foreground = Brushes.White;

						break;
					case "sunny":
						image = new BitmapImage(new Uri("sunWeather.png", UriKind.Relative));
						BackgroundBrush.ImageSource = image;
						WeatherRectangle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4591B8"));

						MyLocation.Foreground = Brushes.Black;
						LocationText.Foreground = Brushes.Black;
						TemperatureText.Foreground = Brushes.Black;
						WeatherText.Foreground = Brushes.Black;
						HigherTemperatureText.Foreground = Brushes.Black;
						LowerTemperatureText.Foreground = Brushes.Black;
						WeatherDescriptionText.Foreground = Brushes.Black;
						TodaysDate.Foreground = Brushes.Black;
						DateText.Foreground = Brushes.Black;

						ZeroHoursText.Foreground = Brushes.Black;
						OneHoursText.Foreground = Brushes.Black;
						TwoHoursText.Foreground = Brushes.Black;
						ThreeHoursText.Foreground = Brushes.Black;
						FourHoursText.Foreground = Brushes.Black;
						FiveHoursText.Foreground = Brushes.Black;
						SixHoursText.Foreground = Brushes.Black;
						SevenHoursText.Foreground = Brushes.Black;
						EightHoursText.Foreground = Brushes.Black;
						NineHoursText.Foreground = Brushes.Black;
						TenHoursText.Foreground = Brushes.Black;
						ElevenHoursText.Foreground = Brushes.Black;
						TwelveHoursText.Foreground = Brushes.Black;
						ThirteenHoursText.Foreground = Brushes.Black;

						FourteenHoursText.Foreground = Brushes.Black;
						FifteenHoursText.Foreground = Brushes.Black;
						SixteenHoursText.Foreground = Brushes.Black;
						SeventeenHoursText.Foreground = Brushes.Black;
						EighteenHoursText.Foreground = Brushes.Black;
						NineteenHoursText.Foreground = Brushes.Black;
						TwentyHoursText.Foreground = Brushes.Black;
						TwentyOneHoursText.Foreground = Brushes.Black;
						TwentyTwoHoursText.Foreground = Brushes.Black;
						TwentyThreeHoursText.Foreground = Brushes.Black;

						break;
					case "rain":
						image = new BitmapImage(new Uri("rainWeather.png", UriKind.Relative));
						BackgroundBrush.ImageSource = image;
						WeatherRectangle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF37506B"));

						MyLocation.Foreground = Brushes.White;
						LocationText.Foreground = Brushes.White;
						TemperatureText.Foreground = Brushes.White;
						WeatherText.Foreground = Brushes.White;
						HigherTemperatureText.Foreground = Brushes.White;
						LowerTemperatureText.Foreground = Brushes.White;
						WeatherDescriptionText.Foreground = Brushes.White;
						TodaysDate.Foreground = Brushes.White;
						DateText.Foreground = Brushes.White;

						ZeroHoursText.Foreground = Brushes.White;
						OneHoursText.Foreground = Brushes.White;
						TwoHoursText.Foreground = Brushes.White;
						ThreeHoursText.Foreground = Brushes.White;
						FourHoursText.Foreground = Brushes.White;
						FiveHoursText.Foreground = Brushes.White;
						SixHoursText.Foreground = Brushes.White;
						SevenHoursText.Foreground = Brushes.White;
						EightHoursText.Foreground = Brushes.White;
						NineHoursText.Foreground = Brushes.White;
						TenHoursText.Foreground = Brushes.White;
						ElevenHoursText.Foreground = Brushes.White;
						TwelveHoursText.Foreground = Brushes.White;
						ThirteenHoursText.Foreground = Brushes.White;

						FourteenHoursText.Foreground = Brushes.White;
						FifteenHoursText.Foreground = Brushes.White;
						SixteenHoursText.Foreground = Brushes.White;
						SeventeenHoursText.Foreground = Brushes.White;
						EighteenHoursText.Foreground = Brushes.White;
						NineteenHoursText.Foreground = Brushes.White;
						TwentyHoursText.Foreground = Brushes.White;
						TwentyOneHoursText.Foreground = Brushes.White;
						TwentyTwoHoursText.Foreground = Brushes.White;
						TwentyThreeHoursText.Foreground = Brushes.White;

						break;
				}

				WeatherText.Text = !string.IsNullOrEmpty(icon)
				? char.ToUpper(icon[0]) + icon.Substring(1)
				: "";
			});

			// зміна бг
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				this.Close();
			}
		}

		// Переміщення вікна мишею
		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ButtonState == MouseButtonState.Pressed)
				this.DragMove();
		}

		private async void Window_Loaded(object sender, RoutedEventArgs e)
		{
			await GetInfo();
		}

		private void RightArrowClick(object sender, MouseButtonEventArgs e)
		{
			OneToThirteenHoursGrid.Visibility = Visibility.Hidden;
			FourteenToTwentyThreeHoursGrid.Visibility = Visibility.Visible;
        }

		private void LeftArrowClick(object sender, MouseButtonEventArgs e)
		{
			FourteenToTwentyThreeHoursGrid.Visibility = Visibility.Hidden;
			OneToThirteenHoursGrid.Visibility = Visibility.Visible;
		}

		private async void RightDataArrowClick(object sender, MouseButtonEventArgs e)
		{
			if (day < 14)
			{
				day += 1;
				await GetInfo();
			}
		}

		private async void LeftDataArrowClick(object sender, MouseButtonEventArgs e)
		{
			if (day > 0)
			{
				day -= 1;
				await GetInfo();
			}
		}

		private async void RightLocationArrowClick(object sender, MouseButtonEventArgs e)
		{
			if (locationCounter < 6)
			{
				locationCounter += 1;
				await GetInfo();
			}
		}

		private async void LeftLocationArrowClick(object sender, MouseButtonEventArgs e)
		{
			if (locationCounter > 0)
			{
				locationCounter -= 1;
				await GetInfo();
			}
		}
	}
}