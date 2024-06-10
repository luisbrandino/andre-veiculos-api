using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVeiculos.Payment.Data;
using Models;
using Repositories;

namespace AndreVeiculos.Payment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentSlipsController : ControllerBase
    {
        private readonly AndreVeiculosPaymentContext _context;

        public PaymentSlipsController(AndreVeiculosPaymentContext context)
        {
            _context = context;
        }

        [Route("/{repositoryType}/slip/{id}")]
        [HttpGet]
        public async Task<ActionResult<PaymentSlip>> GetPix(string repositoryType, int id)
        {
            IBaseRepository<PaymentSlip> repository = GetRepository<PaymentSlip>(repositoryType);

            var slip = await repository.Find(id);

            if (slip == null)
                return NotFound();

            return slip;
        }

        [Route("/{repositoryType}/slip")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentSlip>>> GetPix(string repositoryType)
        {
            IBaseRepository<PaymentSlip> repository = GetRepository<PaymentSlip>(repositoryType);

            return (await repository.Find()).ToList();
        }

        [Route("/{repositoryType}/slip")]
        [HttpPost]
        public async Task<ActionResult<PaymentSlip>> PostPix(string repositoryType, PaymentSlip slip)
        {
            IBaseRepository<PaymentSlip> repository = GetRepository<PaymentSlip>(repositoryType);

            return await repository.Insert(slip);
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

        private bool PaymentSlipExists(int id)
        {
            return (_context.PaymentSlip?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
