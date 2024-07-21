namespace API_CRUDTareas.Models.Identity
{
    /// <summary>
    /// Modelo que representa los datos de registro de un nuevo usuario.
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// Nombre de usuario.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Dirección de correo electrónico del usuario.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        public string Password { get; set; }
    }
}
