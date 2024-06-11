using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVeiculos.FinancialPending.Data;
using Models;
using Repositories;

namespace AndreVeiculos.FinancialPending.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialPendingsController : ControllerBase
    {
        private readonly AndreVeiculosFinancialPendingContext _context;

        public FinancialPendingsController(AndreVeiculosFinancialPendingContext context)
        {
            _context = context;
        }

        [HttpGet("{repositoryType}")]
        public async Task<ActionResult<IEnumerable<Models.FinancialPending>>> GetFinancialPending(string repositoryType)
        {
            var repository = GetRepository<Models.FinancialPending>(repositoryType);

            var financialPending = await repository.FindWith(client => client.Address);

            return ().ToList();
        }

        [HttpGet("{repositoryType}/{identificationNumber}")]
        public async Task<ActionResult<Models.FinancialPending>> GetFinancialPending(string repositoryType, string identificationNumber)
        {
            var repository = GetRepository<Models.FinancialPending>(repositoryType);
            var client = await repository.FindWith(identificationNumber, client => client.Address);

            if (client == null)
                return NotFound();

            return client;
        }

        [HttpPost("{repositoryType}")]
        public async Task<ActionResult<Models.FinancialPending>> PostFinancialPending(string repositoryType, Models.FinancialPending client)
        {
            var repository = GetRepository<Models.FinancialPending>(repositoryType);
            var addressRepository = GetRepository<Models.Address>(repositoryType);

            var address = await addressRepository.Find(client.Address.Id);

            if (address == null)
                return BadRequest("Endereço não cadastrado");

            client.Address = address;

            await repository.Insert(client);

            return client;
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

        private bool FinancialPendingExists(int id)
        {
            return (_context.FinancialPending?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
