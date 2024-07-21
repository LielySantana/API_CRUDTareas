using API_CRUDTareas.Application;
using API_CRUDTareas.Interfaces;
using API_CRUDTareas.Models.Identity;
using API_CRUDTareas.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API_CRUDTareas.Controllers
{
    /// <summary>
    /// Controlador API para la gestión de usuarios.
    /// </summary>
    /// <response code="401">Usuario no autorizado.</response> 
    /// <response code="400">Respuesta si ocurre un error en el Endpoint.</response>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IJwtService _jwtService;

        public UsersController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }


        /// <summary>
        /// Registra un nuevo usuario.
        /// </summary>
        /// <param name="model">Datos del usuario a registrar.</param>
        /// <returns>Resultado de la operación de registrar un nuevo usuario.</returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     POST /api/account/register
        ///     {
        ///         "username": "jjimenez",
        ///         "email": "jjimenez@example.com",
        ///         "password": "P@ssw0rd"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Respuesta si registra correctamente al usuario.</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                var user = new IdentityUser
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new BadRequestResponse(result.Errors.Select(x => x.Description).ToList()));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse(ex));
            }
        }

        /// <summary>
        /// Inicia sesión de usuario.
        /// </summary>
        /// <param name="model">Datos de inicio de sesión del usuario.</param>
        /// <returns>Token JWT si el inicio de sesión es exitoso.</returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     POST /api/account/login
        ///     {
        ///         "username": "jjimenez",
        ///         "password": "P@ssw0rd"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Respuesta si el usuario se autentica correctamente.</response> 
        /// <response code="401">Respuesta si el usuario o contraseña no son correctos.</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {

            try
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user == null)
                {
                    return Unauthorized();
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var token = _jwtService.GenerateJwtToken(model.Username);

                    return Ok(token);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse(ex));
            }

        }

        /// <summary>
        /// Elimina un usuario existente.
        /// </summary>
        /// <param name="userId">ID del usuario a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     DELETE /api/account/{userId}
        ///
        /// </remarks>       
        /// <response code="200">Respuesta si el usuario se elimina correctamente.</response> 
        /// <response code="404">Respuesta si el usuario no es encontrado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new BadRequestResponse(result.Errors.Select(x => x.Description).ToList()));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse(ex));
            }

        }


        /// <summary>
        /// Obtiene todos los usuarios.
        /// </summary>
        /// <returns>Resultado de la operación.</returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     DELETE /api/account/users
        ///
        /// </remarks>       
        /// <response code="200">Respuesta si se encuentra la lista de usuarios.</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [HttpGet("users")]
        public ActionResult<IList<IdentityUser>> GetUsers()
        {
            try
            {
                List<IdentityUser> users = _userManager.Users.Select(user => new IdentityUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                }).ToList();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse(ex));
            }
        }
    }
}
