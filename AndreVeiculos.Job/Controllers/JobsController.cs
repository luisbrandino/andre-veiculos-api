using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVeiculos.Job.Data;
using Models;
using Repositories;

namespace AndreVeiculos.Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly AndreVeiculosJobContext _context;

        public JobsController(AndreVeiculosJobContext context)
        {
            _context = context;
        }

        [HttpGet("{repositoryType}")]
        public async Task<ActionResult<IEnumerable<Models.Job>>> GetJob(string repositoryType)
        {
            var repository = GetRepository<Models.Job>(repositoryType);

            return (await repository.Find()).ToList();
        }

        [HttpGet("{repositoryType}/{id}")]
        public async Task<ActionResult<Models.Job>> GetJob(string repositoryType, int id)
        {
            var repository = GetRepository<Models.Job>(repositoryType);
            var job = await repository.Find(id);

            if (job == null)
                return NotFound();

            return job;
        }

        [HttpPost("{repositoryType}")]
        public async Task<ActionResult<Models.Job>> PostJob(string repositoryType, Models.Job job)
        {
            var repository = GetRepository<Models.Job>(repositoryType);

            await repository.Insert(job);

            return job;
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

        private bool JobExists(int id)
        {
            return (_context.Job?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
