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
                    });
                context.SaveChanges();
                
                var controller = new TemperatureController(context);
                var temp = controller.Get(1);
                Assert.Equal(25, temp.Result.Value.TemperatureC);
            }
        }
    }
}
