using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVeiculos.Relative.Data;
using Models;
using Repositories;

namespace AndreVeiculos.Relative.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelativesController : ControllerBase
    {
        private readonly AndreVeiculosRelativeContext _context;

        public RelativesController(AndreVeiculosRelativeContext context)
        {
            _context = context;
        }

        [HttpGet("{repositoryType}")]
        public async Task<ActionResult<IEnumerable<Models.Relative>>> GetRelative(string repositoryType)
        {
            var repository = GetRepository<Models.Relative>(repositoryType);
            var addressRepository = GetRepository<Models.Address>(repositoryType);

            var relatives = await repository.FindWith(relative => relative.Client, relative => relative.Address);

            foreach (var relative in relatives)
                relative.Client.Address = await addressRepository.Find(relative.Client.Address.Id);

            return relatives.ToList();
        }

        [HttpGet("{repositoryType}/{id}")]
        public async Task<ActionResult<Models.Relative>> GetRelative(string repositoryType, int id)
        {
            var repository = GetRepository<Models.Relative>(repositoryType);
            var relative = await repository.FindWith(id, relative => relative.Client, relative => relative.Address);

            if (relative == null)
                return NotFound();

            return relative;
        }

        [HttpPost("{repositoryType}")]
        public async Task<ActionResult<Models.Relative>> PostRelative(string repositoryType, Models.Relative relative)
        {
            var repository = GetRepository<Models.Relative>(repositoryType);
            var addressRepository = GetRepository<Models.Address>(repositoryType);
            var clientRepository = GetRepository<Models.Client>(repositoryType);

            var address = await addressRepository.Find(relative.Address.Id);

            if (address == null)
                return BadRequest("Endereço não cadastrado");

            relative.Address = address;

            var client = await clientRepository.Find(relative.Client.IdentificationNumber);

            if (client == null)
                return BadRequest("Cliente não cadastrado");

            relative.Client = client;

            await repository.Insert(relative);

            return relative;
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

        private bool RelativeExists(string id)
        {
            return (_context.Relative?.Any(e => e.IdentificationNumber == id)).GetValueOrDefault();
        }
    }
}
