using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVeiculos.Client.Data;
using Models;
using Repositories;

namespace AndreVeiculos.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly AndreVeiculosClientContext _context;

        public ClientsController(AndreVeiculosClientContext context)
        {
            _context = context;
        }

        [HttpGet("{repositoryType}")]
        public async Task<ActionResult<IEnumerable<Models.Client>>> GetClient(string repositoryType)
        {
            var repository = GetRepository<Models.Client>(repositoryType);

            return (await repository.FindWith(client => client.Address)).ToList();
        }

        [HttpGet("{repositoryType}/{identificationNumber}")]
        public async Task<ActionResult<Models.Client>> GetClient(string repositoryType, string identificationNumber)
        {
            var repository = GetRepository<Models.Client>(repositoryType);
            var client = await repository.FindWith(identificationNumber, client => client.Address);

            if (client == null)
                return NotFound();

            return client;
        }

        [HttpPost("{repositoryType}")]
        public async Task<ActionResult<Models.Client>> PostClient(string repositoryType, Models.Client client)
        {
            var repository = GetRepository<Models.Client>(repositoryType);
            var addressRepository = GetRepository<Models.Address>(repositoryType);

            var address = await addressRepository.Find(client.Address.Id);

            if (address == null)
                return BadRequest("Endereço não cadastrado");

            client.Address = address;

            await repository.Insert(client);

            return client;
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

        private bool ClientExists(string id)
        {
            return (_context.Client?.Any(e => e.IdentificationNumber == id)).GetValueOrDefault();
        }
    }
}
