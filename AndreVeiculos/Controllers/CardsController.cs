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
    public class CardsController : ControllerBase
    {
        private readonly AndreVeiculosPaymentContext _context;

        public CardsController(AndreVeiculosPaymentContext context)
        {
            _context = context;
        }

        [Route("/{repositoryType}/cards")]
        [HttpPost]
        public async Task<ActionResult<Card>> PostCard(string repositoryType, Card card)
        {
            IBaseRepository<Card> repository = GetRepository<Card>(repositoryType);

            return await repository.Insert(card);
        }

        [Route("/{repositoryType}/cards")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetCard(string repositoryType)
        {
            IBaseRepository<Card> repository = GetRepository<Card>(repositoryType);

            return (await repository.Find()).ToList();
        }

        [Route("/{repositoryType}/cards/{number}")]
        [HttpGet]
        public async Task<ActionResult<Card?>> GetCard(string repositoryType, string number)
        {
            IBaseRepository<Card> repository = GetRepository<Card>(repositoryType);

            var card = await repository.Find(number);

            if (card == null)
                return NotFound();

            return card;
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

        private bool CardExists(string id)
        {
            return (_context.Card?.Any(e => e.Number == id)).GetValueOrDefault();
        }
    }
}
