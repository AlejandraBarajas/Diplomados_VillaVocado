namespace Diplomados_VillaVocado.Models
{
    public class CargaMateria : AuditableEntity 
    {
        public int Id { get; set; }
        // Relación con Materia
        public int MateriaId { get; set; }  // Llave foránea
        public required Materia Materia { get; set; }  // Propiedad de navegación
        // Relación con el diplomado
        public int DiplomadoId { get; set; }  // Llave foránea
        public required Diplomado Diplomado { get; set; }  // Propiedad de navegación
    }


}
