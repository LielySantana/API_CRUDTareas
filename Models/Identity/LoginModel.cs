namespace API_CRUDTareas.Models.Identity
{
    /// <summary>
    /// Modelo que representa los datos de inicio de sesión de un usuario.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Nombre de usuario.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        public string Password { get; set; }
    }
}
