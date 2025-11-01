using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Diplomados_VillaVocado.Migrations
{
    /// <inheritdoc />
    public partial class InitialAndDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diplomados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UserCreateId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserDeleteId = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModification = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diplomados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Rol = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UserCreateId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserDeleteId = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModification = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    DuracionHoras = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    DiplomadoId = table.Column<int>(type: "int", nullable: false),
                    UserCreateId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserDeleteId = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModification = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materias_Diplomados_DiplomadoId",
                        column: x => x.DiplomadoId,
                        principalTable: "Diplomados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CargaDiplomados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    DiplomadoId = table.Column<int>(type: "int", nullable: false),
                    FechaInscripcion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    FechaCompletado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Calificacion = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    UserCreateId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserDeleteId = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModification = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargaDiplomados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CargaDiplomados_Diplomados_DiplomadoId",
                        column: x => x.DiplomadoId,
                        principalTable: "Diplomados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CargaDiplomados_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Diplomados",
                columns: new[] { "Id", "Activo", "CreatedAt", "DeletedAt", "Descripcion", "FechaFin", "FechaInicio", "LastModification", "Nombre", "UserCreateId", "UserDeleteId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Aprende .NET y React", new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Desarrollo Web Full Stack", 1, null },
                    { 2, true, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Metodologías ágiles para gestión de proyectos", new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Gestión de Proyectos Ágiles", 1, null },
                    { 3, true, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Fundamentos y prácticas de ciberseguridad empresarial", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Seguridad Informática", 1, null }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Activo", "Correo", "CreatedAt", "DeletedAt", "LastModification", "Nombre", "Password", "Rol", "UserCreateId", "UserDeleteId" },
                values: new object[,]
                {
                    { 1, true, "admin@villaavocado.com", new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Administrador", "QWRtaW4xMjMh", 2, 1, null },
                    { 2, true, "juan.perez@villaavocado.com", new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Juan Perez G", "VXNlcjEyMyE=", 1, 1, null },
                    { 3, true, "maria.lopez@villaavocado.com", new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, "María Lopez", "VXNlcjEyMyE=", 1, 1, null }
                });

            migrationBuilder.InsertData(
                table: "CargaDiplomados",
                columns: new[] { "Id", "Calificacion", "CreatedAt", "DeletedAt", "DiplomadoId", "Estado", "FechaCompletado", "FechaInscripcion", "LastModification", "UserCreateId", "UserDeleteId", "UsuarioId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 2, null, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, null, 2 },
                    { 2, null, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, null, 3 }
                });

            migrationBuilder.InsertData(
                table: "Materias",
                columns: new[] { "Id", "Activo", "CreatedAt", "DeletedAt", "Descripcion", "DiplomadoId", "DuracionHoras", "LastModification", "Nombre", "Orden", "UserCreateId", "UserDeleteId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Introducción al desarrollo web con HTML y CSS", 1, 40, null, "Fundamentos de HTML y CSS", 1, 1, null },
                    { 2, true, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Programación con JavaScript moderno y asíncrona", 1, 60, null, "JavaScript Moderno", 2, 1, null },
                    { 3, true, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Desarrollo con React", 1, 50, null, "React Avanzado", 3, 1, null },
                    { 4, true, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Desarrollo de APIs RESTful con .NET", 1, 50, null, "ASP.NET Core Web API", 4, 1, null },
                    { 5, true, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Framework Scrum: roles, eventos y artefactos", 2, 30, null, "Introducción a Scrum", 1, 1, null },
                    { 6, true, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Optimización de flujos de trabajo con Kanban", 2, 25, null, "Kanban y Lean", 2, 1, null },
                    { 7, true, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Comunicación efectiva con interesados del proyecto", 2, 20, null, "Gestión de Stakeholders", 3, 1, null },
                    { 8, true, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Medición de rendimiento en proyectos ágiles", 2, 20, null, "Métricas y KPIs Ágiles", 4, 1, null },
                    { 9, true, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Conceptos básicos de seguridad informática", 3, 35, null, "Fundamentos de Ciberseguridad", 1, 1, null },
                    { 10, true, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Técnicas de pruebas de penetración éticas", 3, 45, null, "Ethical Hacking", 2, 1, null },
                    { 11, true, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "OWASP Top 10 y mejores prácticas", 3, 40, null, "Seguridad en Aplicaciones Web", 3, 1, null },
                    { 12, true, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Identificación y mitigación de riesgos de seguridad", 3, 30, null, "Gestión de Riesgos", 4, 1, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CargaDiplomados_DiplomadoId",
                table: "CargaDiplomados",
                column: "DiplomadoId");

            migrationBuilder.CreateIndex(
                name: "IX_CargaDiplomados_UsuarioId_DiplomadoId",
                table: "CargaDiplomados",
                columns: new[] { "UsuarioId", "DiplomadoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materias_DiplomadoId_Orden",
                table: "Materias",
                columns: new[] { "DiplomadoId", "Orden" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios",
                column: "Correo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CargaDiplomados");

            migrationBuilder.DropTable(
                name: "Materias");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Diplomados");
        }
    }
}
