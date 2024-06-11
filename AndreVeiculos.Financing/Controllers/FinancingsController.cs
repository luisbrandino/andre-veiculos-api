using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVeiculos.Financing.Data;
using Models;
using Repositories;
using MongoServices;
using Microsoft.VisualBasic;

namespace AndreVeiculos.Financing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancingsController : ControllerBase
    {
        private readonly AndreVeiculosFinancingContext _context;
        private readonly BankService _service;

        public FinancingsController(AndreVeiculosFinancingContext context, BankService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet("{repositoryType}")]
        public async Task<ActionResult<IEnumerable<Models.Financing>>> GetFinancing(string repositoryType)
        {
            var repository = GetRepository<Models.Financing>(repositoryType);
            var financings = await repository.FindWith(financing => financing.Sale);
            foreach(var financing in financings)
            {
                financing.Bank = _service.Find(financing.Bank.Cnpj);
            }
            
            return financings.ToList();
        }

        [HttpGet("{repositoryType}/{id}")]
        public async Task<ActionResult<Models.Financing>> GetFinancing(string repositoryType, int id)
        {
            var repository = GetRepository<Models.Financing>(repositoryType);
            var financing = await repository.FindWith(id, financing => financing.Sale);

            if (financing == null)
                return NotFound();

            return financing;
        }

        [HttpPost("{repositoryType}")]
        public async Task<ActionResult<Models.Financing>> PostFinancing(string repositoryType, Models.Financing financing)
        {
            var repository = GetRepository<Models.Financing>(repositoryType);
            var saleRepository = GetRepository<Models.Sale>(repositoryType);

            var sale = await saleRepository.Find(financing.Sale.Id);

            if (sale == null)
                return BadRequest("Venda não cadastrada");

            financing.Sale = sale;

            var bank = _service.Find(financing.Bank.Cnpj);

            if (bank == null)
                return BadRequest("Banco não cadastrado");

            financing.Bank = bank;

            await repository.Insert(financing);

            return financing;
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

        private bool FinancingExists(int id)
        {
            return (_context.Financing?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
