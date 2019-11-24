using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using XWeather.Models;
using XWeather.Views;
using XWeather.ViewModels;
using System.Net.Http;
using Newtonsoft.Json;

namespace XWeather.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;
        private const string APIKey = "6d2cd40b6973466ecf521fe8d69498b0";

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var city = args.SelectedItem as City;
            if (city == null)
                return;
            var viewModel = new ItemDetailViewModel(city);
            using(HttpClient httpClient = new HttpClient())
            {
                var request = await httpClient.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={city.Name}&APPID={APIKey}&units=metric");
                dynamic response = JsonConvert.DeserializeObject(await request.Content.ReadAsStringAsync());
                if (response != null)
                {
                    double timezone = double.Parse(response.timezone.ToString());
                    double sunriseSeconds = double.Parse(response.sys.sunrise.ToString()) + timezone;
                    double sunsetSeconds = double.Parse(response.sys.sunset.ToString()) + timezone;
                    DateTime sunrise = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    sunrise = sunrise.AddSeconds(sunriseSeconds);
                    DateTime sunset = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    sunset = sunset.AddSeconds(sunsetSeconds);
                    viewModel.Weather = new Weather
                    {
                        Date = DateTime.Now.ToLocalTime(),
                        Sunset = sunset,
                        Sunrise = sunrise,
                        Temperature = response.main.temp,
                        Pressure = response.main.pressure,
                        Wind = response.wind.speed
                    };
                }
            }

            await Navigation.PushAsync(new ItemDetailPage(viewModel));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}