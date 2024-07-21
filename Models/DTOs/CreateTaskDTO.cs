using System.ComponentModel.DataAnnotations;

namespace API_CRUDTareas.Models.DTOs
{
    /// <summary>
    /// DTO para la creación de una tarea.
    /// </summary>
    public class CreateTaskDTO
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
