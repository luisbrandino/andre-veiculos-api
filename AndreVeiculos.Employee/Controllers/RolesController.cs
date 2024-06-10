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
    public class RolesController : ControllerBase
    {
        private readonly AndreVeiculosEmployeeContext _context;

        public RolesController(AndreVeiculosEmployeeContext context)
        {
            _context = context;
        }

        [HttpGet("{repositoryType}")]
        public async Task<ActionResult<IEnumerable<Models.Role>>> GetRole(string repositoryType)
        {
            var repository = GetRepository<Models.Role>(repositoryType);

            return (await repository.Find()).ToList();
        }

        [HttpGet("{repositoryType}/{id}")]
        public async Task<ActionResult<Models.Role>> GetRole(string repositoryType, int id)
        {
            var repository = GetRepository<Models.Role>(repositoryType);
            var role = await repository.Find(id);

            if (role == null)
                return NotFound();

            return role;
        }

        [HttpPost("{repositoryType}")]
        public async Task<ActionResult<Models.Role>> PostRole(string repositoryType, Models.Role role)
        {
            var repository = GetRepository<Models.Role>(repositoryType);

            await repository.Insert(role);

            return role;
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

        private bool RoleExists(int id)
        {
            return (_context.Role?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
