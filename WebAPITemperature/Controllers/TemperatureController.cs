using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebAPITemperature.Data;
using WebAPITemperature.Hubs;

namespace WebAPITemperature.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHubContext<DataHub> _dataHubContext;

        public TemperatureController(IHubContext<DataHub> dataHubContext, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dataHubContext = dataHubContext;
        }

        //// GET: api/Temperature
        //[HttpGet(Name = "Get")]
        //public async Task<ActionResult<IEnumerable<Temperature>>> GetProducts()
        //{
        //    return await _dbContext.Temperatures.ToListAsync();
        //}

        // GET: api/Temperature
        // [Authorize]
        [HttpGet(Name = "GetThreeLatest")]
        public async Task<ActionResult<List<Temperature>>> GetThreeLatest()
        {
            int max = _dbContext.Temperatures.Count();
            if (max >= 3)
            {
                await _dataHubContext.Clients.All.SendAsync("ReceiveTemp", _dbContext.Temperatures.OrderByDescending(t => t.TemperatureId).Take(1).ToList());
                return _dbContext.Temperatures.OrderByDescending(t => t.TemperatureId).Take(3).ToList();
            }

            await _dataHubContext.Clients.All.SendAsync("ReceiveTemp", _dbContext.Temperatures.OrderByDescending(t => t.TemperatureId).Take(1).ToList());
            return await _dbContext.Temperatures.ToListAsync();
        }
        // GET: api/Temperature/"Date"
        [HttpGet("{from}/{to}")]
        public ActionResult<List<Temperature>> GetByInterval(DateTime from, DateTime to)
        {
            return _dbContext.Temperatures.Where(t => t.Date >= from && t.Date <= to ).ToList();
        }
        
        //// GET: api/Temperature/"Date"
        [HttpGet("{year}/{month}/{day}")]
        public ActionResult<List<Temperature>> GetByDate(int year, int month, int day)
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
        public async Task<ActionResult<Temperature>> Post(Temperature temp)
        {
            if (temp == null)
            {
                return BadRequest();
            }

            _dbContext.Temperatures.Add(temp);
            await _dbContext.SaveChangesAsync();
            await _dataHubContext.Clients.All.SendAsync("SendTemp", _dbContext.Temperatures.OrderByDescending(t => t.TemperatureId).Take(1).ToList());
            return CreatedAtAction("Get", new { id = temp.TemperatureId }, temp);
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
