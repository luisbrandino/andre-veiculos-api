using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVeiculos.Purchase.Data;
using Models;
using Repositories;

namespace AndreVeiculos.Purchase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly AndreVeiculosPurchaseContext _context;

        public PurchasesController(AndreVeiculosPurchaseContext context)
        {
            _context = context;
        }

        [HttpGet("{repositoryType}")]
        public async Task<ActionResult<IEnumerable<Models.Purchase>>> GetPurchase(string repositoryType)
        {
            var repository = GetRepository<Models.Purchase>(repositoryType);

            return (await repository.FindWith(purchase => purchase.Car)).ToList();
        }

        [HttpGet("{repositoryType}/{id}")]
        public async Task<ActionResult<Models.Purchase>> GetPurchase(string repositoryType, int id)
        {
            var repository = GetRepository<Models.Purchase>(repositoryType);
            var purchase = await repository.FindWith(id, purchase => purchase.Car);

            if (purchase == null)
                return NotFound();

            return purchase;
        }

        [HttpPost("{repositoryType}")]
        public async Task<ActionResult<Models.Purchase>> PostPurchase(string repositoryType, Models.Purchase purchase)
        {
            var repository = GetRepository<Models.Purchase>(repositoryType);
            var carRepository = GetRepository<Models.Car>(repositoryType);

            var car = await carRepository.Find(purchase.Car.LicensePlate);

            if (car == null)
                return BadRequest("Carro não cadastrado");

            purchase.Car = car;

            await repository.Insert(purchase);

            return purchase;
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

        private bool PurchaseExists(int id)
        {
            return (_context.Purchase?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
