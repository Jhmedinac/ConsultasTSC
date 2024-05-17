using ConsultasTSC.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsultasTSC.Data
{
    public class CervezaContext : DbContext
    {

        public CervezaContext(DbContextOptions<CervezaContext> options)
             : base(options)
        {
        }

        // Define tus DbSet para cada modelo aquí
        public DbSet<Cerveza> Cervezas { get; set; }
        // Agrega DbSet para otros modelos si es necesario

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cerveza>().ToTable("Cervezas");
        }
    }

}