using API_CRUDTareas.Application;
using API_CRUDTareas.Interfaces;
using API_CRUDTareas.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;
using static API_CRUDTareas.Application.AppSettings;

namespace API_CRUDTareas.Tests
{
    public class JwtServiceTests
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly JwtService _jwtService;
        private readonly AppSettings _appSettings;

        public JwtServiceTests()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _appSettings = new AppSettings
            {
                Jwt = new JwtSettings
                {
                    SecretKey = "this_is_a_very_secure_key",
                    ExpirationMinutes = 60
                }
            };
            _jwtService = new JwtService(_appSettings, _httpContextAccessorMock.Object);
        }

        [Fact]
        public void GenerateJwtToken_ReturnsValidToken()
        {
            // Arrange
            string username = "testuser";

            // Act
            var token = _jwtService.GenerateJwtToken(username);

            // Assert
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Jwt.SecretKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            // Validate the token
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            var claimUsername = principal.FindFirst(ClaimTypes.Name)?.Value;

            Assert.Equal(username, claimUsername);
            Assert.True(validatedToken.ValidTo > DateTime.UtcNow);
        }

        [Fact]
        public void GetUsernameFromContext_ReturnsUsername_WhenAuthenticated()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "testuser")
            }, "mock");
            context.User = new ClaimsPrincipal(identity);
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(context);

            // Act
            var username = _jwtService.GetUsernameFromContext();

            // Assert
            Assert.Equal("testuser", username);
        }

        [Fact]
        public void GetUsernameFromContext_ReturnsAnonymous_WhenNotAuthenticated()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.User = new ClaimsPrincipal(new ClaimsIdentity());
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(context);

            // Act
            var username = _jwtService.GetUsernameFromContext();

            // Assert
            Assert.Equal("UsuarioAnónimo", username);
        }

        [Fact]
        public void GetUsernameFromContext_ReturnsAnonymous_WhenHttpContextIsNull()
        {
            // Arrange
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

            // Act
            var username = _jwtService.GetUsernameFromContext();

            // Assert
            Assert.Equal("UsuarioAnónimo", username);
        }
    }
}
