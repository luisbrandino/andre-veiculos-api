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
using MessageQueueServices.Producers;
using MessageQueueServices.Messages;
using MessageQueueServices.Abstractions;

namespace AndreVeiculos.Bank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanksController : ControllerBase
    {
        private readonly AndreVeiculosBankContext _context;
        private readonly BankService _service;
        private readonly RabbitMqProducer _producer;

        public BanksController(AndreVeiculosBankContext context, BankService service, RabbitMqProducer producer)
        {
            _context = context;
            _service = service;
            _producer = producer;
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
            var message = HttpContext.RequestServices.GetRequiredService<IMessage>();

            message.Content = bank;

            await _producer.ProduceAsync(message);

            return bank;
        }

    }
}
