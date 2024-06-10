using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AndreVeiculos.Job.Data
{
    public class AndreVeiculosJobContext : DbContext
    {
        public AndreVeiculosJobContext (DbContextOptions<AndreVeiculosJobContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Job> Job { get; set; } = default!;
    }
}
