using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using XWeather.Models;
using XWeather.Services;

namespace XWeather.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage, INotifyPropertyChanged
    {
        public City Item { get; set; }
        public static List<City> AllCities { get; set; }
        private ObservableCollection<City> cities = new ObservableCollection<City>();
        public ObservableCollection<City> Cities 
        { 
            get 
            { 
                return cities; 
            } 
            set 
            { 
                cities = value;
                CitiesList_PropertyChanged(this, new PropertyChangedEventArgs("Cities"));
            } 
        }
        public City SelectedCity { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public IDataStore<City> DataStore => DependencyService.Get<IDataStore<City>>();

        public NewItemPage()
        {
            InitializeComponent();
            CityInput.TextChanged += OnCityInputChange;
            BindingContext = this;
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void OnCityInputChange(object sender, TextChangedEventArgs e)
        {
            Cities = new ObservableCollection<City>(AllCities.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower())).ToList());
        }

        private void CitiesList_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(e.PropertyName));
        }

        public async static Task LoadCitiesList()
        {
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            var jsonFileName = "city.list.json";
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFileName}");
            using (var reader = new System.IO.StreamReader(stream))
            {
                var jsonString = await reader.ReadToEndAsync();
                AllCities = JsonConvert.DeserializeObject<List<City>>(jsonString);
            }
        }

        private async void OnSelectedCity(object sender, SelectedItemChangedEventArgs e)
        {
            await DataStore.AddItemAsync(e.SelectedItem as City);
            MessagingCenter.Send(this, "AddItem", e.SelectedItem as City);
            await Navigation.PopModalAsync();
        }
    }
}