using FinanceControlAPI.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
namespace FinanceControlAPI.Data
{
    public class FinanceControlContext : DbContext
    {
        public FinanceControlContext(DbContextOptions<FinanceControlContext> options) : base(options) { }
        public DbSet<Finance> Finance { get; set; } = null!;

        public DbSet<Users> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações adicionais, como chaves primárias, índices, etc.
            // Exemplo:
            modelBuilder.Entity<Users>().HasKey(u => u.Id);

            modelBuilder.Entity<Finance>().HasKey(u => u.Id);
            // Outras configurações
        }
    }

}
