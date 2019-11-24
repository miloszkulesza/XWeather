using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XWeather.Models;
using XWeather.Services;
using XWeather.Views;

namespace XWeather
{
    public partial class App : Application
    {
        public IDataStore<City> DataStore => DependencyService.Get<IDataStore<City>>();
        private string selectedCitiesPath = Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, "Data");
        private string selectedCitiesFileName = "selected.cities.json";
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override async void OnStart()
        {
            await NewItemPage.LoadCitiesList();
            var selectedCitiesExists = File.Exists(Path.Combine(selectedCitiesPath, selectedCitiesFileName));
            var directoryExists = Directory.Exists(selectedCitiesPath);
            if(selectedCitiesExists)
            {
                var selectedCities = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(Path.Combine(selectedCitiesPath, selectedCitiesFileName)));
                foreach(var city in selectedCities)
                {
                    await DataStore.AddItemAsync(city);
                }
            }
            if(!directoryExists)
            {
                Directory.CreateDirectory(selectedCitiesPath);
            }
        }

        protected override async void OnSleep()
        {
            var selectedCities = await DataStore.GetItemsAsync();
            var stringContent = JsonConvert.SerializeObject(selectedCities);
            File.WriteAllText(Path.Combine(selectedCitiesPath, selectedCitiesFileName), stringContent);
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
