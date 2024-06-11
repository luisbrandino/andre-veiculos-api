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
    public class CategoriesController : ControllerBase
    {
        private readonly AndreVeiculosDriverContext _context;

        public CategoriesController(AndreVeiculosDriverContext context)
        {
            _context = context;
        }

        [HttpGet("{repositoryType}")]
        public async Task<ActionResult<IEnumerable<Models.Category>>> GetCategory(string repositoryType)
        {
            var repository = GetRepository<Models.Category>(repositoryType);

            return (await repository.Find()).ToList();
        }

        [HttpGet("{repositoryType}/{id}")]
        public async Task<ActionResult<Models.Category>> GetCategory(string repositoryType, int id)
        {
            var repository = GetRepository<Models.Category>(repositoryType);
            var category = await repository.Find(id);

            if (category == null)
                return NotFound();

            return category;
        }

        [HttpPost("{repositoryType}")]
        public async Task<ActionResult<Models.Category>> PostCategory(string repositoryType, Models.Category category)
        {
            var repository = GetRepository<Models.Category>(repositoryType);

            await repository.Insert(category);

            return category;
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

        private bool CategoryExists(int id)
        {
            return (_context.Category?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
