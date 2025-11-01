using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Diplomados_VillaVocado.Models
{
    public class VillaAvocadoDbContext : DbContext
    {
        public VillaAvocadoDbContext(DbContextOptions<VillaAvocadoDbContext> options) : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<CargaDiplomado> CargaDiplomados { get; set; }
        public DbSet<Diplomado> Diplomados { get; set; }
        public DbSet<Materia> Materias { get; set; }
         protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
             base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>(entity => {
                entity.Property(e => e.Nombre)
                       .IsRequired()
                       .HasMaxLength(100);
                entity.HasIndex(e => e.Correo).IsUnique();
                entity.Property(e => e.Password)
                   .IsRequired()
                   .HasMaxLength(500);
                entity.Property(e => e.Rol)
                  .IsRequired()
                  .HasConversion<int>();
                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValue(true);
            });
            modelBuilder.Entity<Diplomado>( entity =>
            {
                entity.Property(e => e.Activo)
                   .IsRequired()
                   .HasDefaultValue(true);

                entity.HasMany(d => d.Materias)
                    .WithOne(m => m.Diplomado)
                    .HasForeignKey(m => m.DiplomadoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Materia>(entity => {
                // Índice único: No puede haber dos materias con el mismo orden en el mismo diplomado
                entity.HasIndex(e => new { e.DiplomadoId, e.Orden })
                    .IsUnique();
            });
            modelBuilder.Entity<CargaDiplomado>(entity => {
                entity.Property(e => e.Estado)
                   .IsRequired()
                   .HasConversion<int>()
                   .HasDefaultValue(EstadoDiplomado.Inscrito);
                entity.Property(e => e.Calificacion)
                   .HasPrecision(5, 2);
                // Relación Usuario -> CargaDiplomado
                entity.HasOne(cd => cd.Usuario)
                    .WithMany(u => u.Diplomados)
                    .HasForeignKey(cd => cd.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relación Diplomado -> CargaDiplomado
                entity.HasOne(cd => cd.Diplomado)
                    .WithMany(d => d.Diplomados)
                    .HasForeignKey(cd => cd.DiplomadoId)
                    .OnDelete(DeleteBehavior.Cascade);
                // Índice único: Un usuario no puede inscribirse dos veces al mismo diplomado
                entity.HasIndex(e => new { e.UsuarioId, e.DiplomadoId })
                    .IsUnique();
            });
            // configuraciones personalizadas
            // Llamar al DataSeeder
            DataSeeder.SeedData(modelBuilder);
        }

    }

}
