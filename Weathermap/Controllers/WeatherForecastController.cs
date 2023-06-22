using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Weathermap.Helper;

namespace Weathermap.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IHttpClientFactory httpFactory;

        public WeatherForecastController(IHttpClientFactory httpFactory)
        {
            this.httpFactory = httpFactory;
        }

        private const string apiUrl = "http://api.openweathermap.org/data/2.5/group?id=1580578,1581129,1581297,1581188,1587%20923&units=metric&appid=91b7466cc755db1a94caf6d86a9c788a";
        // GET: HomeController/Details

        [HttpGet(Name = "GetWeather")]
        public async Task<ActionResult<WeatherDTO>> GetWeatherInfoForCityAsync()
        {
            try
            {


                var httpClient = httpFactory.CreateClient();
                var response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(json);
                    var weatherDataResponce = JsonConvert.DeserializeObject<Responce>(json);
                    if (weatherDataResponce == null)
                    {
                        return StatusCode(403);
                    }
                    var result = WeatherHelper.ConvertToWeatherDTO(weatherDataResponce);
                    return Ok(result);
                }

                return StatusCode((int)response.StatusCode);
            } 
            catch(Exception e)
            {
                return Forbid("Have Error");
            }
        }

        

        public class WeatherInfoDTO
        {
            public int CityId { get; set; }
            public string CityName { get; set; }
            public string WeatherMain { get; set; }
            public string WeatherDescription { get; set; }
            public string WeatherIcon { get; set; }
            public int MainTemp { get; set; }
            public int MainHumidity { get; set; }
        }

        public class WeatherDTO
        {
            public WeatherDTO(List<WeatherInfoDTO> data, string message, int statusCode)
            {
                Data = data;
                Message = message;
                StatusCode = statusCode;
            }

            public List<WeatherInfoDTO> Data { get; set; }
            public string Message { get; set; }
            public int StatusCode { get; set; }

        }

        public class Responce
        {
            public int cnt { get; set; }
            public List<WeatherData> List { get; set; } 
        }
        public class WeatherData
        {
            public Coord coord { get; set; }
            public Sys sys { get; set; }
            public List<Weather> weather { get; set; }
            public Main main { get; set; }
            public int visibility { get; set; }
            public Wind wind { get; set; }
            public Clouds clouds { get; set; }
            public double dt { get; set; }
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Coord
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }

        public class Sys
        {
            public string country { get; set; }
            public int timezone { get; set; }
            public double sunrise { get; set; }
            public double sunset { get; set; }
        }

        public class Weather
        {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

        public class Main
        {
            public double temp { get; set; }
            public double feels_like { get; set; }
            public double temp_min { get; set; }
            public double temp_max { get; set; }
            public int pressure { get; set; }
            public int humidity { get; set; }
        }

        public class Wind
        {
            public double speed { get; set; }
            public int deg { get; set; }
        }

        public class Clouds
        {
            public int all { get; set; }
        }
    }
}