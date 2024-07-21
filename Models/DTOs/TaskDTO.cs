using System.ComponentModel.DataAnnotations;

namespace API_CRUDTareas.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) para representar una tarea.
    /// </summary>
    public class TaskDTO
    {
        /// <summary>
        /// Identificador único de la tarea.
        /// </summary>
        public int Id { get; set; }
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


        // Campos de control
        /// <summary>
        /// Fecha en que la tarea fue creada.
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Usuario que creó la tarea.
        /// </summary>
        [MaxLength(50)]
        public string UserCreated { get; set; }
        /// <summary>
        /// Fecha en que la tarea fue modificada.
        /// </summary>
        public DateTime? DateModified { get; set; }
        /// <summary>
        /// Usuario que modificó la tarea.
        /// </summary>
        [MaxLength(50)]
        public string UserModified { get; set; }
        /// <summary>
        /// Nombre de la máquina desde la cual se creó la tarea.
        /// </summary>
        [MaxLength(50)]
        public string MachineCreated { get; set; }
        /// <summary>
        /// Nombre de la máquina desde la cual se modificó la tarea.
        /// </summary>
        [MaxLength(50)]
        public string MachineModified { get; set; }
    }
}
