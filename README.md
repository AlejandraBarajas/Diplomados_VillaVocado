# Diplomados_VillaVocado
#Un usuario puede:
Ver lista de diplomados disponibles (completo)
Inscribirse a diplomados (con validación de fechas* falto CRUD de Diplomados)
Visualizar diplomados asignados (completo)
Ver materias de cada diplomado en orden definido (completo)
Consultar estado de inscripción (completo)
# Un administrador puede
CRUD completo de usuarios (completo)
CRUD completo de diplomados (con validación de fechas)
CRUD completo de materias (con orden secuencial)
Visualizar todos los usuarios y sus diplomados asignados (completo)
Panel de administración intuitivo
Validaciones:
Un usuario no puede inscribirse a diplomados con fechas solapadas
Las materias se muestran en orden secuencial definido
Un diplomado puede tener múltiples materias
Un usuario puede tener múltiples diplomados
#Backend:
•	ASP.NET Core 8.0 - Framework web
•	Entity Framework Core 9.0 - ORM
•	SQL Server LocalDB - Base de datos local
•	C# - Lenguaje de programación
Frontend:
•	Razor Pages / MVC - Motor de vistas
•	Bootstrap 5.3 - Framework CSS
#Requisitos previos
Instalar Visual Studio 2022 (v17.8 o superior)
NET 8.0 SDK
SQL Server LocalDB (incluido con Visual Studio)
Pasos:
1.- Clona el repositorio de Git
git clone https://github.com/AlejandraBarajas/Diplomados_VillaVocado.git
cd Diplomados_VillaVocado
o ejecuta el programa directamente del archivo .ZIP
2.- Restaura paquetes Nuget
3.- Ejecuta Migraciones
Abre la Package Manager Console en Visual Studio y ejecuta:
Update-Database
4.- Ejecuta el proyecto
El proyecto se ejecutará en: https://localhost:7228
#Validaciones
Connection String
La cadena de conexión se encuentra en appsettings.json:
json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=VillaAvocadoDB;Trusted_Connection=True;"
  }
#Uso
Iniciar Sesión
1.	Ve a https://localhost:7228/Account/Login
2.	Usa las credenciales de prueba (ver sección de Credenciales debajo del boton de logeo de la pantalla principal)
Panel de Administrador
Como administrador puedes:
•	Gestionar usuarios (Crear, Editar, Ver, Eliminar)
•	Gestionar diplomados (Crear, Editar, Ver, Eliminar)
•	Gestionar materias (Crear, Editar, Ver, Eliminar)
Panel de Usuario
Como usuario puedes:
•	Ver diplomados disponibles
•	Inscribirte a diplomados
•	Ver tus diplomados asignados con sus materias


