using Microsoft.IdentityModel.Tokens;
using API_CRUDTareas.Application;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API_CRUDTareas.Interfaces;

namespace API_CRUDTareas.Services
{
    /// <summary>
    /// Servicio para la generación de tokens JWT.
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="JwtService"/>.
        /// </summary>
        /// <param name="appSettings">Configuración de la aplicación.</param>
        /// <param name="httpContextAccessor">Accesor para obtener el contexto HTTP actual.</param>
        public JwtService(AppSettings appSettings, IHttpContextAccessor httpContextAccessor)
        {
            _appSettings = appSettings;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Genera un token JWT para un usuario especificado.
        /// </summary>
        /// <param name="username">Nombre de usuario para el cual se genera el token.</param>
        /// <returns>Un token JWT como una cadena de texto.</returns>
        public string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Jwt.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, username),
                }),
                Expires = DateTime.UtcNow.AddMinutes(_appSettings.Jwt.ExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        ///  Método para obtener el nombre de usuario desde el JWT
        /// </summary>
        /// <returns></returns>
        public string GetUsernameFromContext()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null || !httpContext.User.Identity.IsAuthenticated)
            {
                return "UsuarioAnónimo";
            }

            // Obtenemos el nombre de usuario desde las claims del usuario actual
            var usernameClaim = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            return usernameClaim ?? "UsuarioAnónimo";
        }
    }
}
