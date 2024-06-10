using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AndreVeiculos.Payment.Data
{
    public class AndreVeiculosPaymentContext : DbContext
    {
        public AndreVeiculosPaymentContext (DbContextOptions<AndreVeiculosPaymentContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Payment> Payment { get; set; } = default!;

        public DbSet<Models.Card> Card { get; set; } = default!;

        public DbSet<Models.PaymentSlip> PaymentSlip { get; set; }

        public DbSet<Models.Pix> Pix { get; set; } = default!;

        public DbSet<Models.PixType> PixTypes { get; set; } = default!; 
    }
}
