using System;

namespace WebAPITemperature
{
    public class Temperature
    {
        public int TemperatureId { get; set; }

        public DateTime Date { get; set; } = DateTime.Now.Date;

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public int Humidity { get; set; }

        public double Pressure { get; set; }
    }
}
