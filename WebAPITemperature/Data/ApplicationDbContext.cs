using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPITemperature.Models;

namespace WebAPITemperature.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options) { }

            public DbSet<Temperature> Temperatures { get; set; }

            public DbSet<WebAPITemperature.Models.User> User { get; set; }
            
    }
}
