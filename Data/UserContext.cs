using ConsultasTSC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ConsultasTSC.Data
{
    public class UserContext :DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        // Define las propiedades DbSet para cada modelo
        public DbSet<User> Users { get; set; }
        // Agrega otras DbSet según sea necesario para tus modelos
        //public DbSet<OtherModel> OtherModels { get; set; }
        //public DbSet<AnotherModel> AnotherModels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            // Configura el nombre de la tabla para otros modelos si es necesario
            //modelBuilder.Entity<OtherModel>().ToTable("OtherModel");
            //modelBuilder.Entity<AnotherModel>().ToTable("AnotherModel");
        }
    }
}


//public class Db : DbContext
//{
//    public DbSet<Localidades> Localidades { get; set; }
//    public DbSet<Usuario> Usuario { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder
//    optionsBuilder)
//    {
//        optionsBuilder.UseMySQL("server=179.43.XXX.X;database=XXXXX;");
//    }

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        base.OnModelCreating(modelBuilder);

//        modelBuilder.Entity<Localidades>(entity =>
//        {
//            entity.HasKey(e => e.Id_localidad);
//        });
//        modelBuilder.Entity<Usuario>(s => s.HasKey(
//          e => e.Id_usuario
//        ));
//    }
//}