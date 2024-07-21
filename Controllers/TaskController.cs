using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using API_CRUDTareas.Models.Context;
using API_CRUDTareas.Models.DTOs;
using API_CRUDTareas.Interfaces;
using API_CRUDTareas.Models.Responses;
using API_CRUDTareas.Application;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace API_CRUDTareas.Controllers
{
    /// <summary>
    /// Controlador API para la gestión de tareas.
    /// </summary>
    /// <response code="401">Usuario no autorizado.</response> 
    /// <response code="400">Respuesta si ocurre un error en el Endpoint.</response>  
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
  
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly AppSettings _appSettings;

        public TasksController(ITaskService taskService, AppSettings appSettings)
        {
            _taskService = taskService;
            _appSettings = appSettings;
        }


        /// <summary>
        /// Obtiene el listado de todas las tareas.
        /// </summary>
        /// <returns>Lista de las tareas.</returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     GET /api/tasks
        ///
        /// </remarks>
        /// <response code="200">Respuesta si encuentra el listado de tareas.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> GetTasks()
        {
            try
            {
                var tasks = await _taskService.GetTasksAsync();
                return Ok(tasks.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse(ex));
            }
        }


        /// <summary>
        /// Obtiene una tarea por su ID.
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     GET /api/tasks/{id}
        ///
        /// </remarks>
        /// <param name="id">ID de la tarea.</param>
        /// <returns>La tarea encontrada.</returns>
        /// <response code="200">Respuesta si la tarea fue encontrada.</response>
        /// <response code="404">Respuesta si no se encuentra la tarea con el ID especificado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskDTO>> GetTask(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);

                if (task == null)
                {
                    return NotFound();
                }

                return Ok(task);

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse(ex));
            }       
        }


        /// <summary>
        /// Crea una nueva tarea.
        /// </summary>
        /// <param name="createTaskDto">Datos para crear la tarea.</param>
        /// <returns>La tarea creada.</returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     POST /api/tasks
        ///     {
        ///         "title": "Tarea 1",
        ///         "description": "Descripción de la tarea",
        ///         "isCompleted": false
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Respuesta si la tarea fue creada.</response>
        /// <response code="400">Respuesta si ocurre un error en el Endpoint o los datos de la Tarea no son validos.</response>
        [HttpPost]
        [ProducesResponseType(typeof(TaskDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskDTO>> CreateTask(CreateTaskDTO createTaskDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var task = await _taskService.CreateTaskAsync(createTaskDto);

                return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse(ex));
            }  
        }


        /// <summary>
        /// Actualiza una tarea existente.
        /// </summary>
        /// <param name="id">ID de la tarea a actualizar.</param>
        /// <param name="updateTaskDto">Datos actualizados de la tarea.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     PUT /api/tasks/{id}
        ///     {
        ///         "title": "Tarea actualizada",
        ///         "description": "Descripción actualizada de la tarea",
        ///         "isCompleted": true
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Respuesta si la tarea se actualiza correctamente.</response>
        /// <response code="404">Respuesta si no se encuentra la tarea con el ID especificado.</response>
        /// <response code="400">Respuesta si ocurre un error en el Endpoint o los datos de la Tarea no son validos.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTask(int id, UpdateTaskDTO updateTaskDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedTask = await _taskService.UpdateTaskAsync(id, updateTaskDto);

                if (updatedTask == null)
                {
                    return NotFound();
                }

                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse(ex));
            }
           
        }


        /// <summary>
        /// Elimina una tarea existente.
        /// </summary>
        /// <param name="id">ID de la tarea a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     DELETE /api/tasks/{id}
        ///
        /// </remarks>
        /// <response code="204">Respuesta si la tarea se elimina correctamente.</response>
        /// <response code="404">Respuesta si no se encuentra la tarea con el ID especificado.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var result = await _taskService.DeleteTaskAsync(id);

                if (!result)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse(ex));
            }
           
        }
    }

}
