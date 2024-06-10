using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVeiculos.CarJob.Data;
using Models;
using Repositories;

namespace AndreVeiculos.CarJob.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarJobController : ControllerBase
    {
        private readonly AndreVeiculosCarJobContext _context;

        public CarJobController(AndreVeiculosCarJobContext context)
        {
            _context = context;
        }

        [HttpGet("{repositoryType}")]
        public async Task<ActionResult<IEnumerable<Models.CarJob>>> GetCarJob(string repositoryType)
        {
            var repository = GetRepository<Models.CarJob>(repositoryType);

            return (await repository.FindWith(carJob => carJob.Car, carJob => carJob.Job)).ToList();
        }

        [HttpGet("{repositoryType}/{id}")]
        public async Task<ActionResult<Models.CarJob>> GetCarJob(string repositoryType, int id)
        {
            var repository = GetRepository<Models.CarJob>(repositoryType);
            var carJob = await repository.FindWith(id, carJob => carJob.Car, carJob => carJob.Job);

            if (carJob == null)
                return NotFound();

            return carJob;
        }

        [HttpPost("{repositoryType}")]
        public async Task<ActionResult<Models.CarJob>> PostCarJob(string repositoryType, Models.CarJob carJob)
        {
            var repository = GetRepository<Models.CarJob>(repositoryType);
            var carRepository = GetRepository<Models.Car>(repositoryType);
            var jobRepository = GetRepository<Models.Job>(repositoryType);

            var car = await carRepository.Find(carJob.Car.LicensePlate);

            if (car == null)
                return BadRequest("Carro não cadastrado");

            var job = await jobRepository.Find(carJob.Job.Id);

            if (job == null)
                return BadRequest("Serviço não cadastrado");

            carJob.Car = car;
            carJob.Job = job;

            await repository.Insert(carJob);

            return carJob;
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

        private bool CarJobExists(int id)
        {
            return (_context.CarJob?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
