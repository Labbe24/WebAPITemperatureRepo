using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebAPITemperature;


namespace WebApi.Data
{
    public class MemoryRepository : IRepository
    {
        private static MemoryRepository instance = null;
        private readonly Dictionary<long, Temperature> items;

        private MemoryRepository()
        {
            items = new Dictionary<long, Temperature>();

            /*new List<Temperature> {
                new Temperature {TemperatureC = 28, Humidity = 14, Pressure = 10},
                new Temperature {TemperatureC = 18, Humidity = 20, Pressure = 15},
                new Temperature {TemperatureC = 8, Humidity = 22, Pressure = 12}
            }.ForEach(r => AddTemperature(r));*/
        }

        // Implement singleton design pattern
        static public MemoryRepository GetInstance()
        {
            if (instance == null)
                instance = new MemoryRepository();
            return instance;
        }

        public Temperature this[long id] => items.ContainsKey(id) ? items[id] : null;

        public List<Temperature> Temperatures => items.Values.ToList();

        public Temperature AddTemperature(Temperature Temperature)
        {
            if (Temperature.TemperatureId == 0)
            {
                int key = items.Count;
                while (items.ContainsKey(key)) { key++; };
                Temperature.TemperatureId = key;
            }
            items[Temperature.TemperatureId] = Temperature;
            return Temperature;
        }

        public void DeleteTemperature(long id) => items.Remove(id);

        public Temperature UpdateTemperature(Temperature temperature)
            => AddTemperature(temperature);
    }
}