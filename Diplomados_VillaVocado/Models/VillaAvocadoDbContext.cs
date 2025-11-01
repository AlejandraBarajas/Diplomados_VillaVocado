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
        public DbSet<CargaMateria> CargaMaterias { get; set; }
         protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
             base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Diplomado>().ToTable("Diplomados");
            modelBuilder.Entity<Materia>().ToTable("Materias");
            modelBuilder.Entity<CargaMateria>().ToTable("CargaMaterias");
            modelBuilder.Entity<CargaDiplomado>().ToTable("CargaDiplomados");
            // configuraciones personalizadas
        }

    }

}
