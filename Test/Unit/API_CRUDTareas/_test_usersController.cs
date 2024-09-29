using API_CRUDTareas.Controllers;
using API_CRUDTareas.Interfaces;
using API_CRUDTareas.Models.Identity;
using API_CRUDTareas.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace API_CRUDTareas.Tests
{
    public class UsersControllerTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<SignInManager<IdentityUser>> _signInManagerMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _userManagerMock = CreateUserManagerMock();
            _signInManagerMock = CreateSignInManagerMock();
            _jwtServiceMock = new Mock<IJwtService>();
            _controller = new UsersController(_userManagerMock.Object, _signInManagerMock.Object, _jwtServiceMock.Object);
        }

        private Mock<UserManager<IdentityUser>> CreateUserManagerMock()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private Mock<SignInManager<IdentityUser>> CreateSignInManagerMock()
        {
            return new Mock<SignInManager<IdentityUser>>(_userManagerMock.Object, null, null, null, null, null, null);
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenUserIsCreated()
        {
            // Arrange
            var model = new RegisterModel { Username = "jjimenez", Email = "jjimenez@example.com", Password = "P@ssw0rd" };
            var user = new IdentityUser { UserName = model.Username, Email = model.Email };
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), model.Password))
                            .ReturnsAsync(IdentityResult.Success);
            
            // Act
            var result = await _controller.Register(model);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUserCreationFails()
        {
            // Arrange
            var model = new RegisterModel { Username = "jjimenez", Email = "jjimenez@example.com", Password = "P@ssw0rd" };
            var errors = new List<IdentityError> { new IdentityError { Description = "Error en la creación" } };
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), model.Password))
                            .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

            // Act
            var result = await _controller.Register(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<BadRequestResponse>(badRequestResult.Value);
            Assert.Single(response.Errors);
        }

        [Fact]
        public async Task Login_ReturnsOkWithToken_WhenCredentialsAreValid()
        {
            // Arrange
            var model = new LoginModel { Username = "jjimenez", Password = "P@ssw0rd" };
            var user = new IdentityUser { UserName = model.Username };
            _userManagerMock.Setup(um => um.FindByNameAsync(model.Username)).ReturnsAsync(user);
            _signInManagerMock.Setup(sm => sm.PasswordSignInAsync(user, model.Password, false, false))
                              .ReturnsAsync(SignInResult.Success);
            _jwtServiceMock.Setup(js => js.GenerateJwtToken(model.Username)).Returns("token");

            // Act
            var result = await _controller.Login(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("token", okResult.Value);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenUserNotFound()
        {
            // Arrange
            var model = new LoginModel { Username = "jjimenez", Password = "P@ssw0rd" };
            _userManagerMock.Setup(um => um.FindByNameAsync(model.Username)).ReturnsAsync((IdentityUser)null);

            // Act
            var result = await _controller.Login(model);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenPasswordIsIncorrect()
        {
            // Arrange
            var model = new LoginModel { Username = "jjimenez", Password = "P@ssw0rd" };
            var user = new IdentityUser { UserName = model.Username };
            _userManagerMock.Setup(um => um.FindByNameAsync(model.Username)).ReturnsAsync(user);
            _signInManagerMock.Setup(sm => sm.PasswordSignInAsync(user, model.Password, false, false))
                              .ReturnsAsync(SignInResult.Failed);

            // Act
            var result = await _controller.Login(model);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsOk_WhenUserIsDeleted()
        {
            // Arrange
            string userId = "userId";
            var user = new IdentityUser { Id = userId };
            _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            string userId = "userId";
            _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync((IdentityUser)null);

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsBadRequest_WhenDeleteFails()
        {
            // Arrange
            string userId = "userId";
            var user = new IdentityUser { Id = userId };
            var errors = new List<IdentityError> { new IdentityError { Description = "Error en la eliminación" } };
            _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.DeleteAsync(user)).ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<BadRequestResponse>(badRequestResult.Value);
            Assert.Single(response.Errors);
        }

        [Fact]
        public void GetUsers_ReturnsOk_WithListOfUsers()
        {
            // Arrange
            var users = new List<IdentityUser>
            {
                new IdentityUser { Id = "1", UserName = "user1", Email = "user1@example.com" },
                new IdentityUser { Id = "2", UserName = "user2", Email = "user2@example.com" }
            };
            _userManagerMock.Setup(um => um.Users).Returns(users.AsQueryable());

            // Act
            var result = _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsAssignableFrom<List<IdentityUser>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count);
        }

        [Fact]
        public void GetUsers_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            _userManagerMock.Setup(um => um.Users).Throws(new Exception("Error al obtener usuarios"));

            // Act
            var result = _controller.GetUsers();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<BadRequestResponse>(badRequestResult.Value);
            Assert.NotNull(response);
        }
    }
}
