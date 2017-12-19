using HELP.UI.Responsible.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ResponsibleTest
{
    public class HomeControllerTest
    {

        [Fact]
        public void About()
        {
            // Arrange
            var controller = new HomeController();
            //var errorView = "~/Views/Home/About.cshtml";

            // Act
            //var homeController = new HomeController();
            //homeController.ControllerContext.HttpContext = _httpContextMock.Object;
            // var actionResult = homeController.Request.Path;
            var result = controller.About();
            // result.StatusCode

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal("About", viewResult.ViewName);
        }



    }
}
