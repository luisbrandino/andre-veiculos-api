using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AndreVeiculos.Relative.Data
{
    public class AndreVeiculosRelativeContext : DbContext
    {
        public AndreVeiculosRelativeContext (DbContextOptions<AndreVeiculosRelativeContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Relative> Relative { get; set; } = default!;
    }
}
