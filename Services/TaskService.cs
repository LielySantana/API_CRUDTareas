using API_CRUDTareas.Interfaces;
using API_CRUDTareas.Models.Context;
using API_CRUDTareas.Models.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API_CRUDTareas.Services
{
    /// <summary>
    /// Servicio para la gestión de tareas.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly TasksContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="TaskService"/>.
        /// </summary>
        /// <param name="context">Contexto de la base de datos.</param>
        /// <param name="mapper">Instancia de AutoMapper.</param>
        /// <param name="jwtService">Instancia de jwtService.</param>
        public TaskService(TasksContext context, 
            IMapper mapper,
            IJwtService jwtService)
        {
            _context = context;
            _mapper = mapper;
            _jwtService = jwtService;   
        }

        /// <summary>
        /// Obtiene una lista de todas las tareas.
        /// </summary>
        /// <returns>Una lista de <see cref="TaskDTO"/>.</returns>
        public async Task<IEnumerable<TaskDTO>> GetTasksAsync()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return _mapper.Map<IEnumerable<TaskDTO>>(tasks);
        }

        /// <summary>
        /// Obtiene una tarea por su ID.
        /// </summary>
        /// <param name="id">ID de la tarea.</param>
        /// <returns>Una instancia de <see cref="TaskDTO"/>.</returns>
        public async Task<TaskDTO> GetTaskByIdAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            return _mapper.Map<TaskDTO>(task);
        }

        /// <summary>
        /// Crea una nueva tarea.
        /// </summary>
        /// <param name="createTaskDTO">Datos para crear la tarea.</param>
        /// <returns>Una instancia de <see cref="TaskDTO"/> con la tarea creada.</returns>
        public async Task<TaskDTO> CreateTaskAsync(CreateTaskDTO createTaskDTO)
        {
            var task = _mapper.Map<TaskModel>(createTaskDTO);
            task.DateCreated = DateTime.UtcNow;
            task.UserCreated = _jwtService.GetUsernameFromContext();
            task.MachineCreated = Environment.MachineName;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return _mapper.Map<TaskDTO>(task);
        }

        /// <summary>
        /// Actualiza una tarea existente.
        /// </summary>
        /// <param name="id">ID de la tarea a actualizar.</param>
        /// <param name="updateTaskDTO">Datos actualizados de la tarea.</param>
        /// <returns>Una instancia actualizada de <see cref="TaskDTO"/>.</returns>
        public async Task<TaskDTO> UpdateTaskAsync(int id, UpdateTaskDTO updateTaskDTO)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return null;
            }

            _mapper.Map(updateTaskDTO, task);
            task.DateModified = DateTime.UtcNow;
            task.UserModified = _jwtService.GetUsernameFromContext();
            task.MachineModified = Environment.MachineName;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return _mapper.Map<TaskDTO>(task);
        }

        /// <summary>
        /// Elimina una tarea por su ID.
        /// </summary>
        /// <param name="id">ID de la tarea a eliminar.</param>
        /// <returns>Verdadero si la tarea fue eliminada; de lo contrario, falso.</returns>
        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return false; 
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return true;
        }
    }

}
