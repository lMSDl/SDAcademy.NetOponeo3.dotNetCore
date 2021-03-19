using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using Mvc.Controllers;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mvc.Test.Controllers
{
    public class UsersControllerUnitTest
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithEmptyListOfUsers()
        {
            //Arrage
            var service = new Mock<IUsersServiceAsync>();
            service.Setup(x => x.FindAsync(It.IsAny<string>(), It.IsAny<Roles?>()))
                .ReturnsAsync(() => new List<User>());

            var controller = new UsersController(service.Object);

            //Act
            var result = await controller.Index(It.IsAny<string>(), It.IsAny<Roles?>());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(viewResult.Model);
            Assert.Empty(model);
        }

        [Fact]
        public async Task Delete_ResturnsBadRequest_WhenIdIsNull()
        {
            //Arrange
            var controller = new UsersController(null);

            //Act
            var result = await controller.Delete(null);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_ResturnsNotFound_WhenIdNotExists()
        {
            //Arrange
            var service = new Mock<IUsersServiceAsync>();
            service.Setup(x => x.ReadAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            var controller = new UsersController(service.Object);

            //Act
            var result = await controller.Delete((int?)It.IsAny<int>());

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ResturnsAViewResult_WithSelectedUser()
        {
            //Arrange
            var service = new Mock<IUsersServiceAsync>();
            service.Setup(x => x.ReadAsync(It.IsAny<int>()))
                .ReturnsAsync(() => new User());

            var controller = new UsersController(service.Object);

            //Act
            var result = await controller.Delete((int?)It.IsAny<int>());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<User>(viewResult.Model);
            Assert.NotNull(viewResult.Model);
        }

        [Fact]
        public async Task DeletePost_ResturnsRedirect_DeletesUser()
        {
            //Arrange
            var mockService = new Mock<IUsersServiceAsync>();
            mockService.Setup(x => x.DeleteAsync(It.IsAny<int>())).Verifiable();

            var controller = new UsersController(mockService.Object);

            //Act
            var result = await controller.Delete(It.IsAny<int>());

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Null(redirectToActionResult.ControllerName);
            mockService.Verify();
        }

        [Fact]
        public async Task AddPost_ResturnsAViewResult_WithModelStateError()
        {
            //Arrange
            var controller = new UsersController(null);
            controller.ModelState.AddModelError(nameof(User.Login), "Required");
            var user = It.IsAny<User>();

            //Act
            var result = await controller.Add(user);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(user, viewResult.Model);
            Assert.False(viewResult.ViewData.ModelState.IsValid);
        }
    }
}
