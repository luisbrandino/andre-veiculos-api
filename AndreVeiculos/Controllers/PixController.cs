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
    public class PixController : ControllerBase
    {
        private readonly AndreVeiculosPaymentContext _context;

        public PixController(AndreVeiculosPaymentContext context)
        {
            _context = context;
        }

        [Route("/{repositoryType}/pix/{id}")]
        [HttpGet]
        public async Task<ActionResult<Pix>> GetPix(string repositoryType, int id)
        {
            IBaseRepository<Pix> repository = GetRepository<Pix>(repositoryType);

            var pix = await repository.FindWith(id, p => p.Type);

            if (pix == null)
                return NotFound();

            return pix;
        }

        [Route("/{repositoryType}/pix")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pix>>> GetPix(string repositoryType)
        {
            IBaseRepository<Pix> repository = GetRepository<Pix>(repositoryType);

            return (await repository.FindWith(p => p.Type)).ToList();
        }

        [Route("/{repositoryType}/pix")]
        [HttpPost]
        public async Task<ActionResult<Pix>> PostPix(string repositoryType, Pix pix)
        {
            IBaseRepository<Pix> repository = GetRepository<Pix>(repositoryType);
            IBaseRepository<PixType> pixTypeRepository = GetRepository<PixType>(repositoryType);

            var pixType = await pixTypeRepository.Find(pix.Type.Id);

            if (pixType == null)
                return BadRequest("Tipo de pix inexistente");

            pix.Type = pixType;

            return await repository.Insert(pix);
        }

        [Route("/{repositoryType}/pixType")]
        [HttpPost]
        public async Task<ActionResult<PixType>> PostPixType(string repositoryType, PixType pixType)
        {
            IBaseRepository<PixType> repository = GetRepository<PixType>(repositoryType);

            return await repository.Insert(pixType);
        }

        [Route("/{repositoryType}/pixType")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PixType>>> GetPixType(string repositoryType)
        {
            IBaseRepository<PixType> repository = GetRepository<PixType>(repositoryType);

            return (await repository.Find()).ToList();
        }

        [Route("/{repositoryType}/pixType/{id}")]
        [HttpGet]
        public async Task<ActionResult<PixType>> GetPixType(string repositoryType, int id)
        {
            IBaseRepository<PixType> repository = GetRepository<PixType>(repositoryType);

            var pixType = await repository.Find(id);

            if (pixType == null)
                return NotFound();

            return pixType;
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

        private bool PixExists(int id)
        {
            return (_context.Pix?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
