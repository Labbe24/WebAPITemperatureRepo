using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebAPITemperature.Data;

namespace WebAPITemperature.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public TemperatureController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //// GET: api/Temperature
        [HttpGet(Name = "Get")]
        public async Task<ActionResult<IEnumerable<Temperature>>> GetProducts()
        {
            return await _dbContext.Temperatures.ToListAsync();
        }

        // GET: api/Temperature
        [HttpGet(Name = "GetThreeLatest")]
        public async Task<ActionResult<List<Temperature>>> GetThreeLatest()
        {
            int max = _dbContext.Temperatures.Count();
            if (max >= 3)
            {
                return _dbContext.Temperatures.OrderBy(t => t.Date).Take(3).ToList();
            }

            return await _dbContext.Temperatures.ToListAsync();
        }
        // GET: api/Temperature/"Date"
        [HttpGet("{from}/{to}")]
        public ActionResult<List<Temperature>> GetByInterval(DateTime from, DateTime to)
        {
            return _dbContext.Temperatures.Where(t => t.Date >= from && t.Date <= to ).ToList();
        }

        //// GET: api/Temperature/"Date"
        [HttpGet("{day, month, year}")]
        public ActionResult<List<Temperature>> GetByDate(int day, int month, int year)
        {
            return _dbContext.Temperatures.Where(t => t.Date.Day == day && t.Date.Month == month && t.Date.Year == year).ToList();
        }

        // GET: api/Temperature/5
        [HttpGet("{id}", Name = "GetById")]
        public async Task<ActionResult<Temperature>> Get(int id)
        {
            var product = await _dbContext.Temperatures.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/Temperature
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        public ActionResult<Temperature> Post(Temperature temp)
        {
            if (temp == null)
            {
                return BadRequest();
            }
            var newTemp = _dbContext.Add(new Temperature
            {
                TemperatureC = temp.TemperatureC,
                Humidity = temp.Humidity,
                Pressure = temp.Pressure
            });

            return CreatedAtAction("Get", new { id = newTemp.Entity }, newTemp);
        }

        // PUT: api/Temperature/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(long id, Temperature temperature)
        {
            if (id != temperature.TemperatureId)
            {
                return BadRequest();
            }

            _dbContext.Entry(temperature).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TemperatureExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Temperature/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Temperature>> DeleteProduct(long id)
        {
            var product = await _dbContext.Temperatures.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Temperatures.Remove(product);
            await _dbContext.SaveChangesAsync();

            return product;
        }

        private bool TemperatureExist(long id)
        {
            return _dbContext.Temperatures.Any(e => e.TemperatureId == id);
        }
    }
}
