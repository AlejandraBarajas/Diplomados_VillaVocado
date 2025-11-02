namespace Diplomados_VillaVocado.Models
{
    public class AuditableEntity
    {
        // Campos de control
        public int? UserCreateId { get; set; }  // ID del usuario que crea
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Fecha de creacion
        public int? UserDeleteId { get; set; }  // ID del usuario que elimina
        public DateTime? DeletedAt { get; set; } // Fecha de eliminación (puede ser null)
        public DateTime? LastModification { get; set; } // Fecha de última modificación
                                                        // Clase genérica EntityFrameworkBase que incluye un Id de tipo primario
        public class EntityBase : AuditableEntity
        {
            public int Id { get; set; }  // Identificador principal

        }
    }
}
