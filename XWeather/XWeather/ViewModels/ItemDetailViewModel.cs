using System;

using XWeather.Models;

namespace XWeather.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public City Item { get; set; }
        public Weather Weather { get; set; }
        public ItemDetailViewModel(City item = null, Weather weather = null)
        {
            Title = item?.Name;
            Item = item;
            Weather = weather;
        }
    }
}
