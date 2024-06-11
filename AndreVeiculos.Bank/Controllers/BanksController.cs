using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVeiculos.Bank.Data;
using Models;
using Repositories;
using MongoServices;

namespace AndreVeiculos.Bank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanksController : ControllerBase
    {
        private readonly AndreVeiculosBankContext _context;
        private readonly BankService _service;

        public BanksController(AndreVeiculosBankContext context, BankService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Bank>>> GetBank()
        {
            return _service.Find();
        }

        [HttpGet("{cnpj}")]
        public async Task<ActionResult<Models.Bank>> GetBank(string cnpj)
        {
            var bank = _service.Find(cnpj);

            if (bank == null)
                return NotFound();

            return bank;
        }

        [HttpPost]
        public async Task<ActionResult<Models.Bank>> PostBank(Models.Bank bank)
        {
            _service.Insert(bank);

            return bank;
        }

    }
}
