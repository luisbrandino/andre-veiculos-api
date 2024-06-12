using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVeiculos.TermsOfUse.Data;
using Models;
using MongoServices;
using Repositories;

namespace AndreVeiculos.TermsOfUse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TermsOfUseAcceptancesController : ControllerBase
    {
        private readonly AndreVeiculosTermsOfUseContext _context;
        private readonly TermsOfUseAcceptanceService _service;
        private readonly TermsOfUseService _termsOfUseService;

        public TermsOfUseAcceptancesController(AndreVeiculosTermsOfUseContext context, TermsOfUseAcceptanceService service, TermsOfUseService termsOfUseService)
        {
            _context = context;
            _service = service;
            _termsOfUseService = termsOfUseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.TermsOfUseAcceptance>>> Get()
        {
            return _service.Find();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.TermsOfUseAcceptance>> GetTermsOfUse(string id)
        {
            var termsOfUseAcceptance = _service.Find(id);

            if (termsOfUseAcceptance == null)
                return NotFound();

            return termsOfUseAcceptance;
        }

        [HttpPost]
        public async Task<ActionResult<Models.TermsOfUseAcceptance>> PostTermsOfUse(Models.TermsOfUseAcceptance termsOfUseAcceptance)
        {
            termsOfUseAcceptance.Id = null;

            var termsOfUse = _termsOfUseService.Find(termsOfUseAcceptance.TermsOfUse.Id);

            if (termsOfUse == null)
                return BadRequest("Termos de uso não cadastrado");

            termsOfUseAcceptance.TermsOfUse = termsOfUse;

            var clientRepository = GetRepository<Client>("dapper");

            var client = await clientRepository.Find(termsOfUseAcceptance.Client.IdentificationNumber);

            if (client == null)
                return BadRequest("Cliente não cadastrado");

            termsOfUseAcceptance.Client = client;

            _service.Insert(termsOfUseAcceptance);

            return termsOfUseAcceptance;
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
    }
}
