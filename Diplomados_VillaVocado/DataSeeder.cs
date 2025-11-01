using Diplomados_VillaVocado.Models;
using Microsoft.EntityFrameworkCore;

namespace Diplomados_VillaVocado
{
    public static class DataSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            var now = new DateTime(2025, 11, 1, 12, 0, 0);

            modelBuilder.Entity<Usuario>().HasData(
                new 
                {
                    Id = 1,
                    Nombre = "Administrador",
                    Correo = "admin@villaavocado.com",
                    Password = HashPassword("Admin123!"),
                    Rol = RolUsuario.Administrador,
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                },
                new 
                {
                    Id = 2,
                    Nombre = "Juan Perez G",
                    Correo = "juan.perez@villaavocado.com",
                    Password = HashPassword("User123!"),
                    Rol = RolUsuario.Usuario,
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                },
                new 
                {
                    Id = 3,
                    Nombre = "María Lopez",
                    Correo = "maria.lopez@villaavocado.com",
                    Password = HashPassword("User123!"),
                    Rol = RolUsuario.Usuario,
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                }
            );
            modelBuilder.Entity<Diplomado>().HasData(
                new 
                {
                    Id = 1,
                    Nombre = "Desarrollo Web Full Stack",
                    Descripcion = "Aprende .NET y React",
                    FechaInicio = new DateTime(2025, 1, 15),
                    FechaFin = new DateTime(2025, 6, 15),
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                },
                new 
                {
                    Id = 2,
                    Nombre = "Gestión de Proyectos Ágiles",
                    Descripcion = "Metodologías ágiles para gestión de proyectos",
                    FechaInicio = new DateTime(2025, 2, 1),
                    FechaFin = new DateTime(2025, 5, 1),
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                },
                new 
                {
                    Id = 3,
                    Nombre = "Seguridad Informática",
                    Descripcion = "Fundamentos y prácticas de ciberseguridad empresarial",
                    FechaInicio = new DateTime(2025, 7, 1),
                    FechaFin = new DateTime(2025, 12, 1),
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                }
            );

            modelBuilder.Entity<Materia>().HasData(
                new 
                {
                    Id = 1,
                    Nombre = "Fundamentos de HTML y CSS",
                    Descripcion = "Introducción al desarrollo web con HTML y CSS",
                    Orden = 1,
                    DuracionHoras = 40,
                    DiplomadoId = 1,
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                },
                new 
                {
                    Id = 2,
                    Nombre = "JavaScript Moderno",
                    Descripcion = "Programación con JavaScript moderno y asíncrona",
                    Orden = 2,
                    DuracionHoras = 60,
                    DiplomadoId = 1,
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                },
                new 
                {
                    Id = 3,
                    Nombre = "React Avanzado",
                    Descripcion = "Desarrollo con React",
                    Orden = 3,
                    DuracionHoras = 50,
                    DiplomadoId = 1,
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                },
                new 
                {
                    Id = 4,
                    Nombre = "ASP.NET Core Web API",
                    Descripcion = "Desarrollo de APIs RESTful con .NET",
                    Orden = 4,
                    DuracionHoras = 50,
                    DiplomadoId = 1,
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                }
            );

            modelBuilder.Entity<Materia>().HasData(
                new 
                {
                    Id = 5,
                    Nombre = "Introducción a Scrum",
                    Descripcion = "Framework Scrum: roles, eventos y artefactos",
                    Orden = 1,
                    DuracionHoras = 30,
                    DiplomadoId = 2,
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                },
                new 
                {
                    Id = 6,
                    Nombre = "Kanban y Lean",
                    Descripcion = "Optimización de flujos de trabajo con Kanban",
                    Orden = 2,
                    DuracionHoras = 25,
                    DiplomadoId = 2,
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                },
                new 
                {
                    Id = 7,
                    Nombre = "Gestión de Stakeholders",
                    Descripcion = "Comunicación efectiva con interesados del proyecto",
                    Orden = 3,
                    DuracionHoras = 20,
                    DiplomadoId = 2,
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                },
                new 
                {
                    Id = 8,
                    Nombre = "Métricas y KPIs Ágiles",
                    Descripcion = "Medición de rendimiento en proyectos ágiles",
                    Orden = 4,
                    DuracionHoras = 20,
                    DiplomadoId = 2,
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                }
            );
            modelBuilder.Entity<Materia>().HasData(
                new 
                {
                    Id = 9,
                    Nombre = "Fundamentos de Ciberseguridad",
                    Descripcion = "Conceptos básicos de seguridad informática",
                    Orden = 1,
                    DuracionHoras = 35,
                    DiplomadoId = 3,
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                },
                new 
                {
                    Id = 10,
                    Nombre = "Ethical Hacking",
                    Descripcion = "Técnicas de pruebas de penetración éticas",
                    Orden = 2,
                    DuracionHoras = 45,
                    DiplomadoId = 3,
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                },
                new 
                {
                    Id = 11,
                    Nombre = "Seguridad en Aplicaciones Web",
                    Descripcion = "OWASP Top 10 y mejores prácticas",
                    Orden = 3,
                    DuracionHoras = 40,
                    DiplomadoId = 3,
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                },
                new 
                {
                    Id = 12,
                    Nombre = "Gestión de Riesgos",
                    Descripcion = "Identificación y mitigación de riesgos de seguridad",
                    Orden = 4,
                    DuracionHoras = 30,
                    DiplomadoId = 3,
                    Activo = true,
                    CreatedAt = now,
                    UserCreateId = 1
                }
            );
            modelBuilder.Entity<CargaDiplomado>().HasData(
                new 
                {
                    Id = 1,
                    UsuarioId = 2,
                    DiplomadoId = 1,
                    FechaInscripcion = new DateTime(2025, 1, 10),
                    Estado = EstadoDiplomado.EnCurso,
                    CreatedAt = now,
                    UserCreateId = 1
                },
                new 
                {
                    Id = 2,
                    UsuarioId = 3,
                    DiplomadoId = 2,
                    FechaInscripcion = new DateTime(2025, 1, 25),
                    Estado = EstadoDiplomado.Inscrito,
                    CreatedAt = now,
                    UserCreateId = 1
                }
            );
        }

        // Método helper para hashear passwords
        private static string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
