using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WebAPITemperature.Controllers;
using WebAPITemperature.Data;
using Xunit;

namespace WebAPITemperature.Tests
{
    public class TemperatureControllerTests
    {
        [Fact]
        public void GetLatestActionListisCorrect()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.Temperatures.AddRange(
                    new Temperature
                    {
                        TemperatureC = 25,
                        Humidity = 10,
                        Pressure = 30
                    });
                context.SaveChanges();
                
                var controller = new TemperatureController(null, context);
                var temp = controller.Get(1);
                Assert.Equal(25, temp.Result.Value.TemperatureC);
            }
        }

        [Fact]
        public void GetThreeLatestActionListisCorrect()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.Temperatures.AddRange(
                    new Temperature
                    {
                        TemperatureC = 25,
                        Humidity = 10,
                        Pressure = 30
                    },
                    new Temperature
                    {
                        TemperatureC = 24,
                        Humidity = 9,
                        Pressure = 5
                    },
                    new Temperature
                    {
                        TemperatureC = 23,
                        Humidity = 2,
                        Pressure = 1
                    });
                context.SaveChanges();

                var controller = new TemperatureController(null, context);
                var date1 = DateTime.Now.AddDays(-1);
                var date2 = DateTime.Now.AddDays(1);
                var temps = controller.GetByInterval(date1, date2);

                Assert.Equal(25,temps.Value[0].TemperatureC);
                Assert.Equal(24,temps.Value[1].TemperatureC);
                Assert.Equal(23,temps.Value[2].TemperatureC);
            }
        }
    }
}
