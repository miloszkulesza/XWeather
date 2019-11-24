using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XWeather.Models;

namespace XWeather.Services
{
    public class MockDataStore : IDataStore<City>
    {
        readonly List<City> items;

        public MockDataStore()
        {
            items = new List<City>();
        }

        public async Task<bool> AddItemAsync(City item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(City item)
        {
            var oldItem = items.Where((City arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var oldItem = items.Where((City arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<City> GetItemAsync(int id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<City>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}