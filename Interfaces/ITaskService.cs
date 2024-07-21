using API_CRUDTareas.Models.DTOs;

namespace API_CRUDTareas.Interfaces
{
    /// <summary>
    /// Define las operaciones disponibles en la interface para la gestión de tareas.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Obtiene todas las tareas disponibles.
        /// </summary>
        /// <returns>Una colección de tareas representadas como objetos TaskDTO.</returns>
        Task<IEnumerable<TaskDTO>> GetTasksAsync();
        /// <summary>
        /// Obtiene una tarea específica por su identificador.
        /// </summary>
        /// <param name="id">Identificador único de la tarea.</param>
        /// <returns>La tarea encontrada representada como un objeto TaskDTO, o null si no se encuentra.</returns>
        Task<TaskDTO> GetTaskByIdAsync(int id);
        /// <summary>
        /// Crea una nueva tarea con los datos proporcionados.
        /// </summary>
        /// <param name="createTaskDto">Datos de la tarea a crear.</param>
        /// <returns>La nueva tarea creada representada como un objeto TaskDTO.</returns>
        Task<TaskDTO> CreateTaskAsync(CreateTaskDTO createTaskDto);
        /// <summary>
        /// Actualiza una tarea existente identificada por su ID, utilizando los datos proporcionados.
        /// </summary>
        /// <param name="id">Identificador único de la tarea a actualizar.</param>
        /// <param name="updateTaskDto">Datos actualizados de la tarea.</param>
        /// <returns>La tarea actualizada representada como un objeto TaskDTO, o null si la tarea no existe.</returns>
        Task<TaskDTO> UpdateTaskAsync(int id, UpdateTaskDTO updateTaskDto);
        /// <summary>
        /// Elimina una tarea existente identificada por su ID.
        /// </summary>
        /// <param name="id">Identificador único de la tarea a eliminar.</param>
        /// <returns>True si la tarea se eliminó correctamente, False si la tarea no existe.</returns>
        Task<bool> DeleteTaskAsync(int id);
    }
}
