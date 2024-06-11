using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVeiculos.Insurance.Data;
using Models;
using Repositories;

namespace AndreVeiculos.Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsurancesController : ControllerBase
    {
        private readonly AndreVeiculosInsuranceContext _context;

        public InsurancesController(AndreVeiculosInsuranceContext context)
        {
            _context = context;
        }

        [HttpGet("{repositoryType}")]
        public async Task<ActionResult<IEnumerable<Models.Insurance>>> GetInsurance(string repositoryType)
        {
            var repository = GetRepository<Models.Insurance>(repositoryType);
            var clientRepository = GetRepository<Client>(repositoryType);
            var carRepository = GetRepository<Car>(repositoryType);
            var driverRepository = GetRepository<Driver>(repositoryType);
            var addressRepository = GetRepository<Address>(repositoryType);
            var licenseRepository = GetRepository<DriverLicense>(repositoryType);

            var insurances = (await repository.FindWith(
                insurance => insurance.Client,
                insurance => insurance.Car,
                insurance => insurance.Driver
            )).ToList();

            foreach (var insurance in insurances)
            {
                insurance.Client.Address = await addressRepository.Find(insurance.Client.Address.Id);
                insurance.Driver.Address = await addressRepository.Find(insurance.Driver.Address.Id);
                insurance.Driver.License = await licenseRepository.Find(insurance.Driver.License.Id);
            }

            return insurances;
        }

        [HttpGet("{repositoryType}/{id}")]
        public async Task<ActionResult<Models.Insurance>> GetInsurance(string repositoryType, int id)
        {
            var repository = GetRepository<Models.Insurance>(repositoryType);
            var clientRepository = GetRepository<Client>(repositoryType);
            var carRepository = GetRepository<Car>(repositoryType);
            var driverRepository = GetRepository<Driver>(repositoryType);
            var addressRepository = GetRepository<Address>(repositoryType);
            var licenseRepository = GetRepository<DriverLicense>(repositoryType);

            var insurance = (await repository.FindWith(id, 
                insurance => insurance.Client,
                insurance => insurance.Car,
                insurance => insurance.Driver
            ));

            insurance.Client.Address = await addressRepository.Find(insurance.Client.Address.Id);
            insurance.Driver.Address = await addressRepository.Find(insurance.Driver.Address.Id);
            insurance.Driver.License = await licenseRepository.Find(insurance.Driver.License.Id);

            return insurance;
        }

        [HttpPost("{repositoryType}")]
        public async Task<ActionResult<Models.Insurance>> PostInsurance(string repositoryType, Models.Insurance insurance)
        {
            var repository = GetRepository<Models.Insurance>(repositoryType);
            var clientRepository = GetRepository<Client>(repositoryType);

            var client = await clientRepository.Find(insurance.Client.IdentificationNumber);

            if (client == null)
                return BadRequest("Cliente não cadastrado");

            insurance.Client = client;

            var driverRepository = GetRepository<Driver>(repositoryType);

            var driver = await driverRepository.Find(insurance.Driver.IdentificationNumber);

            if (driver == null) 
                return BadRequest("Condutor não cadastrado");

            var carRepository = GetRepository<Models.Car>(repositoryType);

            var car = await carRepository.Find(insurance.Car.LicensePlate);

            if (car == null)
                return BadRequest("Carro não cadastrado");

            insurance.Car = car;

            await repository.Insert(insurance);

            return insurance;
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

        private bool InsuranceExists(int id)
        {
            return (_context.Insurance?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
