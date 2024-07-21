using API_CRUDTareas.Models.Context;
using AutoMapper;

namespace API_CRUDTareas.Models.DTOs
{
    /// <summary>
    /// Perfil de mapeo de AutoMapper para las entidades y DTOs de tareas.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MappingProfile"/> y define los mapeos.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<TaskModel, TaskDTO>().ReverseMap();
            CreateMap<TaskModel, CreateTaskDTO>().ReverseMap();
            CreateMap<TaskModel, UpdateTaskDTO>().ReverseMap();
        }
    }
}
