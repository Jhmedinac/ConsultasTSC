using ConsultasTSC.Models;
using Microsoft.EntityFrameworkCore;
namespace ConsultasTSC.Data
{
    public class OrganigramaContext: DbContext
    {
        public OrganigramaContext(DbContextOptions<OrganigramaContext> options) : base(options)
        {
        }

        public DbSet<Organigrama> Organigramas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organigrama>(entity =>
            {
                entity.ToTable("Organigrama", "cat");

                entity.HasKey(e => e.CodigoOrganigrama)
                      .HasName("Organigrama_PK");

                entity.Property(e => e.CodigoOrganigrama)
                      .HasColumnName("codigo_organigrama");

                entity.Property(e => e.DescripcionOrganigrama)
                      .HasColumnName("descripcion_organigrama")
                      .HasMaxLength(250)
                      .IsRequired();

                entity.Property(e => e.CodigoOrganigramaPadre)
                      .HasColumnName("codigo_organigrama_padre");

                entity.Property(e => e.Estado)
                      .HasColumnName("estado")
                      .IsRequired();

                entity.Property(e => e.FechaCreacion)
                      .HasColumnName("fecha_creacion")
                      .IsRequired();

                entity.Property(e => e.FechaModificacion)
                      .HasColumnName("fecha_modificacion");

                entity.Property(e => e.UsuarioCreacion)
                      .HasColumnName("usuario_creacion")
                      .IsRequired();

                entity.Property(e => e.UsuarioModificacion)
                      .HasColumnName("usuario_modificacion");
            });
        }
    }
}