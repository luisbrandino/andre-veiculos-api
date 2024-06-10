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
    public class PaymentsController : ControllerBase
    {
        private readonly AndreVeiculosPaymentContext _context;

        public PaymentsController(AndreVeiculosPaymentContext context)
        {
            _context = context;
        }

        [Route("/{repositoryType}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Payment>>> GetPayment(string repositoryType)
        {
            var repository = GetRepository<Models.Payment>(repositoryType);
            var pixRepository = GetRepository<Models.Pix>(repositoryType);

            var payments = (await repository.FindWith(
                payment => payment.Card,
                payment => payment.PaymentSlip,
                payment => payment.Pix
            )).ToList();

            foreach (var payment in payments)
                if (payment.Pix != null)
                    payment.Pix = await pixRepository.FindWith(payment.Pix.Id, pix => pix.Type);

            return payments;
        }

        [Route("/{repositoryType}/{id}")]
        [HttpGet]
        public async Task<ActionResult<Models.Payment>> GetPayment(string repositoryType, int id)
        {
            var repository = GetRepository<Models.Payment>(repositoryType);

            var payment = await repository.FindWith(id,
                payment => payment.Card,
                payment => payment.PaymentSlip,
                payment => payment.Pix
            );

            if (payment == null)
                return NotFound();

            var pixRepository = GetRepository<Models.Pix>(repositoryType);

            payment.Pix = await pixRepository.FindWith(payment.Pix.Id, pix => pix.Type);

            return payment;
        }

        [Route("/{repositoryType}")]
        [HttpPost]
        public async Task<ActionResult<Models.Payment>> PostPayment(string repositoryType, Models.Payment payment)
        {
            IBaseRepository<Models.Payment> repository = GetRepository<Models.Payment>(repositoryType);

            bool hasPaymentMethod = payment.Card != null || payment.PaymentSlip != null || payment.Pix != null;

            if (!hasPaymentMethod)
                return BadRequest("Nenhum método de pagamento foi especificado");

            if (payment.Card == null)
            {
                await repository.Insert(payment);
                return payment;
            }

            IBaseRepository<Card> cardRepository = GetRepository<Card>(repositoryType);

            var card = await cardRepository.Find(payment.Card.Number);

            if (card == null)
                return BadRequest("Cartão de crédito não registrado");

            payment.Card = card;

            await repository.Insert(payment);

            return payment;
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

        private bool PaymentExists(int id)
        {
            return (_context.Payment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
