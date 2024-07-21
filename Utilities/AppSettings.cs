namespace API_CRUDTareas.Application
{
    /// <summary>
    /// Clase que encapsula la configuración de la aplicación.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Configuración de autenticación JWT.
        /// </summary>
        public JwtSettings Jwt { get; set; }

        /// <summary>
        /// Clase que encapsula la configuración de autenticación JWT.
        /// </summary>
        public class JwtSettings
        {
            /// <summary>
            /// Clave secreta utilizada para firmar el token JWT.
            /// </summary>
            public string SecretKey { get; set; }
            /// <summary>
            /// Tiempo de expiración del token JWT en minutos.
            /// </summary>
            public int ExpirationMinutes { get; set; }
        }
    }
}
