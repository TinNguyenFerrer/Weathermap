
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using Weathermap.Controllers;

namespace TestQuanLyDIem.Controllers
{
    public class WeatherForecastControllerTest
    {
        [Fact]
        public async Task GetWeatherInfoForCity_CallAPIOpenWeathermapSuccess_ReturnSuccessful()
        {
            // Arrange
            string url = "http://api.openweathermap.org/data/2.5/group?id=1580578,1581129,1581297,1581188,1587%20923&units=metric&appid=91b7466cc755db1a94caf6d86a9c788a";
            string jsonResponse = "{\"data\":[],\"message\":\"Test\"}";
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpClient = new Mock<HttpClient>();

            mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse),
            });

            mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

            var weatherController = new WeatherForecastController(mockHttpClientFactory.Object);

            // Act
            var result = await weatherController.GetWeatherInfoForCityAsync();

            // Assert
            Assert.NotNull(result);
            var objectResult = result.Result as ObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);

        }

        [Fact]
        public async Task GetWeatherInfoForCity_ReturnsForbiddenStatus_returnForbiddenStatus()
        {
            // Arrange
            string url = "http://api.openweathermap.org/data/2.5/group?id=1580578,1581129,1581297,1581188,1587%20923&units=metric&appid=91b7466cc755db1a94caf6d86a9c788a";
            string jsonResponse = "{\"data\":[],\"message\":\"Test\"}";
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpClient = new Mock<HttpClient>();
            mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

            mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Forbidden,
                Content = new StringContent(jsonResponse),
            });

            var weatherController = new WeatherForecastController(mockHttpClientFactory.Object);

            // Act
            var result = await weatherController.GetWeatherInfoForCityAsync();

            // Assert
            Assert.NotNull(result);
            //result.Should().BeOfType<BadRequestResult>();
            var objectResult = result.Result as ObjectResult;
            Assert.Equal((int)HttpStatusCode.Forbidden, objectResult.StatusCode);
        }
    }
}
