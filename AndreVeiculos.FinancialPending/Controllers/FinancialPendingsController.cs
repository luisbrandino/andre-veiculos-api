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
            var addressRepository = GetRepository<Models.Address>(repositoryType);

            var financialPendings = await repository.FindWith(financialPending => financialPending.Client);

            foreach (var financialPending in financialPendings)
                financialPending.Client.Address = await addressRepository.Find(financialPending.Client.Address.Id);

            return financialPendings.ToList();
        }

        [HttpGet("{repositoryType}/{identificationNumber}")]
        public async Task<ActionResult<Models.FinancialPending>> GetFinancialPending(string repositoryType, string identificationNumber)
        {
            var repository = GetRepository<Models.FinancialPending>(repositoryType);
            var addressRepository = GetRepository<Models.Address>(repositoryType);

            var financialPending = await repository.FindWith(identificationNumber, financialPending => financialPending.Client);
            financialPending.Client.Address = await addressRepository.Find(financialPending.Client.Address.Id);

            return financialPending;
        }

        [HttpPost("{repositoryType}")]
        public async Task<ActionResult<Models.FinancialPending>> PostFinancialPending(string repositoryType, Models.FinancialPending financialPending)
        {
            var repository = GetRepository<Models.FinancialPending>(repositoryType);
            var clientRepository = GetRepository<Models.Client>(repositoryType);

            var client = await clientRepository.Find(financialPending.Client.IdentificationNumber);

            if (client == null)
                return BadRequest("Cliente não cadastrado");

            financialPending.Client = client;

            await repository.Insert(financialPending);

            return financialPending;
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
