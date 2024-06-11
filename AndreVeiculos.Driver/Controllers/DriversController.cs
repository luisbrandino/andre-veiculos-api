using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVeiculos.Driver.Data;
using Models;
using Repositories;
using System.ComponentModel;

namespace AndreVeiculos.Driver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly AndreVeiculosDriverContext _context;

        public DriversController(AndreVeiculosDriverContext context)
        {
            _context = context;
        }

        [HttpGet("{repositoryType}")]
        public async Task<ActionResult<IEnumerable<Models.Driver>>> GetDriver(string repositoryType)
        {
            var repository = GetRepository<Models.Driver>(repositoryType);

            return (await repository.FindWith(driver => driver.License)).ToList();
        }

        [HttpGet("{repositoryType}/{id}")]
        public async Task<ActionResult<Models.Driver>> GetDriver(string repositoryType, int id)
        {
            var repository = GetRepository<Models.Driver>(repositoryType);
            var driver = await repository.FindWith(id, driver => driver.License);

            if (driver == null)
                return NotFound();

            return driver;
        }

        [HttpPost("{repositoryType}")]
        public async Task<ActionResult<Models.Driver>> PostDriver(string repositoryType, Models.Driver driver)
        {
            var repository = GetRepository<Models.Driver>(repositoryType);
            var addressRepository = GetRepository<Models.Address>(repositoryType);

            var address = await addressRepository.Find(driver.Address.Id);

            if (address == null)
                return BadRequest();

            driver.Address = address;

            var driverLicenseRepository = GetRepository<Models.DriverLicense>(repositoryType);

            var driverLicense = await driverLicenseRepository.Find(driver.License.Id);

            if (driverLicense == null)
                return BadRequest();

            driver.License = driverLicense;

            await repository.Insert(driver);

            return driver;
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

        private bool DriverExists(string id)
        {
            return (_context.Driver?.Any(e => e.IdentificationNumber == id)).GetValueOrDefault();
        }
    }
}
