using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVeiculos.Employee.Data;
using Models;
using Repositories;

namespace AndreVeiculos.Employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly AndreVeiculosEmployeeContext _context;

        public EmployeesController(AndreVeiculosEmployeeContext context)
        {
            _context = context;
        }

        [HttpGet("{repositoryType}")]
        public async Task<ActionResult<IEnumerable<Models.Employee>>> GetEmployee(string repositoryType)
        {
            var repository = GetRepository<Models.Employee>(repositoryType);

            return (await repository.FindWith(employee => employee.Role, employee => employee.Address)).ToList();
        }

        [HttpGet("{repositoryType}/{identificationNumber}")]
        public async Task<ActionResult<Models.Employee>> GetEmployee(string repositoryType, string identificationNumber)
        {
            var repository = GetRepository<Models.Employee>(repositoryType);
            var employee = await repository.FindWith(identificationNumber, employee => employee.Role, employee => employee.Address);

            if (employee == null)
                return NotFound();

            return employee;
        }

        [HttpPost("{repositoryType}")]
        public async Task<ActionResult<Models.Employee>> PostEmployee(string repositoryType, Models.Employee employee)
        {
            var repository = GetRepository<Models.Employee>(repositoryType);
            var roleRepository = GetRepository<Models.Role>(repositoryType);

            var role = await roleRepository.Find(employee.Role.Id);

            if (role == null)
                return BadRequest("Cargo não registrado");

            employee.Role = role;

            var addressRepository = GetRepository<Models.Address>(repositoryType);

            var address = await addressRepository.Find(employee.Address.Id);

            if (address == null)
                return BadRequest("Endereço não cadastrado");

            employee.Address = address;

            await repository.Insert(employee);

            return employee;
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

        private bool EmployeeExists(string id)
        {
            return (_context.Employee?.Any(e => e.IdentificationNumber == id)).GetValueOrDefault();
        }
    }
}
