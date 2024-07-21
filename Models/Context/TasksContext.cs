using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API_CRUDTareas.Models.Context
{
    /// <summary>
    /// Contexto de base de datos para la gestión de tareas, incluyendo la autenticación de usuarios.
    /// </summary>
    public class TasksContext : IdentityDbContext<IdentityUser>
    {
        /// <summary>
        /// Constructor que inicializa una nueva instancia del contexto de base de datos de tareas.
        /// </summary>
        /// <param name="options">Opciones para configurar el contexto de base de datos.</param>
        public TasksContext(DbContextOptions<TasksContext> options) : base(options)
        {

        }

        /// <summary>
        /// DbSet que representa la colección de tareas en la base de datos.
        /// </summary>
        public DbSet<TaskModel> Tasks { get; set; }

        /// <summary>
        /// Configuración adicional del modelo de datos.
        /// </summary>
        /// <param name="builder">Constructor de modelos para configurar.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
