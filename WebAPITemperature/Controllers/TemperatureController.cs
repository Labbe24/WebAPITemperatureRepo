using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;

namespace WebAPITemperature.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private MemoryRepository repository;

        public TemperatureController()
        {
            repository = MemoryRepository.GetInstance();
        }

        // GET: api/Temperature
        [HttpGet]
        public ActionResult<List<Temperature>> Get()
        {
            return repository.Temperatures;
        }

        // GET: api/Temperature/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<Temperature> Get(int id)
        {
            var item = repository[id];
            if (item == null)
            {
                return NotFound();
            }
            return item;
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
            var newTemp = repository.AddTemperature(new Temperature
            {
                TemperatureC = temp.TemperatureC,
                Humidity = temp.Humidity,
                Pressure = temp.Pressure
            });
            return CreatedAtAction("Get", new { id = newTemp.TemperatureId }, newTemp);
        }

        // PUT: api/Temperature/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, Temperature temp)
        {
            if (temp == null || temp.TemperatureId != id)
            {
                return BadRequest();
            }
            var item = repository[id];
            if (item == null)
            {
                return NotFound();
            }
            repository.UpdateTemperature(temp);
            return new NoContentResult();
        }

        // DELETE: api/Temperature/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var item = repository[id];
            if (item == null)
            {
                return NotFound();
            }

            repository.DeleteTemperature(id);
            return new NoContentResult();
        }
    }
}
