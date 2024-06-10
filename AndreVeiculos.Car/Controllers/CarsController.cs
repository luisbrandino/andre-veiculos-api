using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVeiculos.Car.Data;
using Models;
using Repositories;

namespace AndreVeiculos.Car.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly AndreVeiculosCarContext _context;

        public CarsController(AndreVeiculosCarContext context)
        {
            _context = context;
        }

        [HttpGet("{repositoryType}")]
        public async Task<ActionResult<IEnumerable<Models.Car>>> GetCar(string repositoryType)
        {
            var repository = GetRepository<Models.Car>(repositoryType);

            return (await repository.Find()).ToList();
        }

        [HttpGet("{repositoryType}/{licensePlate}")]
        public async Task<ActionResult<Models.Car>> GetCar(string repositoryType, string licensePlate)
        {
            var repository = GetRepository<Models.Car>(repositoryType);
            var car = await repository.Find(licensePlate);

            if (car == null)
                return NotFound();

            return car;
        }

        [HttpPost("{repositoryType}")]
        public async Task<ActionResult<Models.Car>> PostCar(string repositoryType, Models.Car car)
        {
            var repository = GetRepository<Models.Car>(repositoryType);
            
            await repository.Insert(car);

            return car;
        }

        public IBaseRepository<T> GetRepository<T>(string type) where T : Model, new()
        {
            return type switch
            {
                "ef" => new EntityFrameworkRepository<T>(_context),
                "dapper" => new DapperRepository<T>(),
                "ado" => new AdoRepository<T>()
            };
        }

        private bool CarExists(string id)
        {
            return (_context.Car?.Any(e => e.LicensePlate == id)).GetValueOrDefault();
        }
    }
}
