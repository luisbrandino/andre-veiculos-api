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
    public class TermsOfUsesController : ControllerBase
    {
        private readonly AndreVeiculosTermsOfUseContext _context;
        private readonly TermsOfUseService _service;

        public TermsOfUsesController(AndreVeiculosTermsOfUseContext context, TermsOfUseService service)
        {
            _context = context;
            _service = service; 
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.TermsOfUse>>> Get()
        {
            return _service.Find();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.TermsOfUse>> GetTermsOfUse(string id)
        {
            var termsOfUse = _service.Find(id);

            if (termsOfUse == null)
                return NotFound();

            return termsOfUse;
        }

        [HttpPost]
        public async Task<ActionResult<Models.TermsOfUse>> PostTermsOfUse(Models.TermsOfUse termsOfUse)
        {
            termsOfUse.Id = null;

            _service.Insert(termsOfUse);

            return termsOfUse;
        }
        
    }
}
