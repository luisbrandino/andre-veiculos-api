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

namespace AndreVeiculos.Driver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverLicensesController : ControllerBase
    {
        private readonly AndreVeiculosDriverContext _context;

        public DriverLicensesController(AndreVeiculosDriverContext context)
        {
            _context = context;
        }

        [HttpGet("{repositoryType}")]
        public async Task<ActionResult<IEnumerable<Models.DriverLicense>>> GetDriverLicense(string repositoryType)
        {
            var repository = GetRepository<Models.DriverLicense>(repositoryType);

            return (await repository.FindWith(license => license.Category)).ToList();
        }

        [HttpGet("{repositoryType}/{id}")]
        public async Task<ActionResult<Models.DriverLicense>> GetDriverLicense(string repositoryType, int id)
        {
            var repository = GetRepository<Models.DriverLicense>(repositoryType);
            var driverLicense = await repository.FindWith(id, license => license.Category);

            if (driverLicense == null)
                return NotFound();

            return driverLicense;
        }

        [HttpPost("{repositoryType}")]
        public async Task<ActionResult<Models.DriverLicense>> PostDriverLicense(string repositoryType, Models.DriverLicense driverLicense)
        {
            var repository = GetRepository<Models.DriverLicense>(repositoryType);
            var categoryRepository = GetRepository<Category>(repositoryType);

            var category = await categoryRepository.Find(driverLicense.Category.Id);

            if (category == null)
                return BadRequest("Categoria não existe");

            driverLicense.Category = category;

            await repository.Insert(driverLicense);

            return driverLicense;
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

        private bool DriverLicenseExists(int id)
        {
            return (_context.DriverLicense?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
