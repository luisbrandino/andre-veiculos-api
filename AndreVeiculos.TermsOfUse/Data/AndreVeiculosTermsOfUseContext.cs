using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AndreVeiculos.TermsOfUse.Data
{
    public class AndreVeiculosTermsOfUseContext : DbContext
    {
        public AndreVeiculosTermsOfUseContext (DbContextOptions<AndreVeiculosTermsOfUseContext> options)
            : base(options)
        {
        }

        public DbSet<Models.TermsOfUse> TermsOfUse { get; set; } = default!;

        public DbSet<Models.TermsOfUseAcceptance>? TermsOfUseAcceptance { get; set; }
    }
}
