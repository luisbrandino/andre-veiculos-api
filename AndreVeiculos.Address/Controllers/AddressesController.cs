using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVeiculos.Address.Data;
using Models;
using Repositories;
using System.Net;
using AndreVeiculos.Address.DTO;
using AndreVeiculos.Address.PostalCodeServices;
using AndreVeiculos.Address.MongoServices;

namespace AndreVeiculos.Address.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly AndreVeiculosAddressContext _context;

        public AddressesController(AndreVeiculosAddressContext context)
        {
            _context = context;
        }

        [HttpGet("{repositoryType}")]
        public async Task<ActionResult<IEnumerable<Models.Address>>> GetCar(string repositoryType)
        {
            var repository = GetRepository<Models.Address>(repositoryType);

            return (await repository.Find()).ToList();
        }

        [HttpGet("{repositoryType}/{id}")]
        public async Task<ActionResult<Models.Address>> GetCar(string repositoryType, int id)
        {
            var repository = GetRepository<Models.Address>(repositoryType);
            var address = await repository.Find(id);

            if (address == null)
                return NotFound();

            return address;
        }

        [HttpPost("{repositoryType}")]
        public async Task<ActionResult<Models.Address>> PostCar(string repositoryType, AddressDTO addressDTO)
        {
            IPostalCodeService postalCodeService = new ViaCepService();
            IAddressResult? result = await postalCodeService.Fetch(addressDTO.PostalCode);

            if (result == null)
                return BadRequest("CEP inválido");

            var repository = GetRepository<Models.Address>(repositoryType);
            var mongoRepository = HttpContext.RequestServices.GetRequiredService<AddressService>();

            var address = new Models.Address()
            {
                Street = result.Street,
                StreetType = result.StreetType,
                State = result.State,
                City = result.City,
                PostalCode = result.PostalCode,
                Neighborhood = result.Neighborhood,
                Number = addressDTO.Number,
                Complement = addressDTO.Complement, 
            };

            address = await repository.Insert(address);
            mongoRepository.Insert(address);

            return address;
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

        private bool AddressExists(int id)
        {
            return (_context.Address?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
