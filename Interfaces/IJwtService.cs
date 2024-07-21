namespace API_CRUDTareas.Interfaces
{

    /// <summary>
    /// Define las operaciones necesarias para la generación y gestión de tokens JWT.
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Genera un token JWT basado en el nombre de usuario proporcionado.
        /// </summary>
        /// <param name="username">Nombre de usuario para el cual se genera el token.</param>
        /// <returns>El token JWT generado como una cadena.</returns>
        string GenerateJwtToken(string username);
        /// <summary>
        /// Obtiene el nombre de usuario desde el contexto actual.
        /// </summary>
        /// <returns>El nombre de usuario extraído del contexto actual.</returns>
        string GetUsernameFromContext();
    }
}
