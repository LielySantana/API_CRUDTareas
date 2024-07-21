using System.ComponentModel.DataAnnotations;

namespace API_CRUDTareas.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) para actualizar una tarea existente.
    /// </summary>
    public class UpdateTaskDTO
    {
        /// <summary>
        /// Título de la tarea.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        /// <summary>
        /// Descripción de la tarea.
        /// </summary>
        [MaxLength(100)]
        public string Description { get; set; }
        /// <summary>
        /// Indica si la tarea está completada.
        /// </summary>
        public bool IsCompleted { get; set; }
    }
}
