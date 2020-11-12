using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPITemperature;

namespace WebApi.Models
{
    public interface IRepository
    {
        List<Temperature> Temperatures { get; }
        Temperature this[long id] { get; }
        Temperature AddTemperature(Temperature Temperature);
        Temperature UpdateTemperature(Temperature Temperature);
        void DeleteTemperature(long id);
    }
}