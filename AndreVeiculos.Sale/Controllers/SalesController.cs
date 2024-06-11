using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVeiculos.Sale.Data;
using Models;
using Repositories;

namespace AndreVeiculos.Sale.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly AndreVeiculosSaleContext _context;

        public SalesController(AndreVeiculosSaleContext context)
        {
            _context = context;
        }

        [HttpGet("{repositoryType}")]
        public async Task<ActionResult<IEnumerable<Models.Sale>>> GetSale(string repositoryType)
        {
            var repository = GetRepository<Models.Sale>(repositoryType);
            var paymentRepository = GetRepository<Payment>(repositoryType);
            var addressRepository = GetRepository<Address>(repositoryType);

            var sales =  (await repository.FindWith(
                sale => sale.Client,
                sale => sale.Employee,
                sale => sale.Car,
                sale => sale.Payment
            )).ToList();

            foreach (var sale in sales)
            {
                sale.Payment = await paymentRepository.FindWith(sale.Payment.Id,
                    payment => payment.Card,
                    payment => payment.PaymentSlip,
                    payment => payment.Pix
                ) ?? new Payment();

                sale.Client.Address = await addressRepository.Find(sale.Client.Address.Id);
                sale.Employee.Address = await addressRepository.Find(sale.Employee.Address.Id);
            }

            return sales;
        }

        [HttpGet("{repositoryType}/{id}")]
        public async Task<ActionResult<Models.Sale>> GetSale(string repositoryType, int id)
        {
            var repository = GetRepository<Models.Sale>(repositoryType);

            var sale = await repository.FindWith(id,
                sale => sale.Client,
                sale => sale.Employee,
                sale => sale.Car,
                sale => sale.Payment
            );

            if (sale == null)
                return NotFound();

            var paymentRepository = GetRepository<Models.Payment>(repositoryType);
            var addressRepository = GetRepository<Address>(repositoryType);

            sale.Payment = await paymentRepository.FindWith(sale.Payment.Id,
                    payment => payment.Card,
                    payment => payment.PaymentSlip,
                    payment => payment.Pix
                ) ?? new Payment();


            sale.Client.Address = await addressRepository.Find(sale.Client.Address.Id);
            sale.Employee.Address = await addressRepository.Find(sale.Employee.Address.Id);

            return sale;
        }

        [HttpPost("{repositoryType}")]
        public async Task<ActionResult<Models.Sale>> PostSale(string repositoryType, Models.Sale sale)
        {
            var repository = GetRepository<Models.Sale>(repositoryType);

            var clientRepository = GetRepository<Models.Client>(repositoryType);

            var client = await clientRepository.FindWith(sale.Client.IdentificationNumber, client => client.Address);

            if (client == null)
                return BadRequest("Cliente não cadastrado");

            sale.Client = client;

            var employeeRepository = GetRepository<Models.Employee>(repositoryType);

            var employee = await employeeRepository.FindWith(sale.Employee.IdentificationNumber, employee => employee.Address);

            if (employee == null)
                return BadRequest("Funcionário não cadastrado");

            sale.Employee = employee;

            var carRepository = GetRepository<Models.Car>(repositoryType);

            var car = await carRepository.Find(sale.Car.LicensePlate);

            if (car == null)
                return BadRequest("Carro não cadastrado");

            sale.Car = car;

            var paymentRepository = GetRepository<Models.Payment>(repositoryType);

            var payment = await paymentRepository.FindWith(sale.Payment.Id,
                payment => payment.Card,
                payment => payment.PaymentSlip,
                payment => payment.Pix
            );

            if (payment == null)
                return BadRequest("Pagamento não cadastrado");

            sale.Payment = payment;

            await repository.Insert(sale);

            return sale;
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

        private bool SaleExists(int id)
        {
            return (_context.Sale?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
