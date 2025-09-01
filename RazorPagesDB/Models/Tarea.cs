using System.ComponentModel.DataAnnotations;

namespace RazorPagesDB.Models
{
    public class Tarea : IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name = "Nombre de la tarea")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(80, ErrorMessage = "Máximo 80 caracteres.")]
        public string nombreTarea { get; set; } = string.Empty;

        [Display(Name = "Fecha de vencimiento")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
        public DateTime fechaVencimiento { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El estado es obligatorio.")]
        [RegularExpression("Pendiente|En Progreso|Completada",
            ErrorMessage = "Estado inválido. Use: Pendiente, En Progreso o Completada.")]
        public string estado { get; set; } = "Pendiente";

        [Display(Name = "Usuario")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un usuario válido.")]
        public int IdUsuario { get; set; }

        // Validación adicional: fecha >= hoy
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (fechaVencimiento.Date < DateTime.Today)
            {
                yield return new ValidationResult(
                    "La fecha de vencimiento no puede ser anterior a hoy.",
                    new[] { nameof(fechaVencimiento) });
            }
        }
    }
}
