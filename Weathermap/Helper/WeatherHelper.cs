using static Weathermap.Controllers.WeatherForecastController;

namespace Weathermap.Helper
{
    public static class WeatherHelper
    {
        public static WeatherDTO ConvertToWeatherDTO(Responce weatherResponce)
        {
            var message = "Current weather information of cities";
            var statusCode = 200;

            WeatherDTO weatherDTO = new WeatherDTO(new List<WeatherInfoDTO>(), message, statusCode);
            weatherResponce.List.ForEach((WeatherData weatherData) =>
            {
                WeatherInfoDTO weatherInfoDTO = new WeatherInfoDTO();
                weatherInfoDTO.CityId = weatherData.id;
                weatherInfoDTO.CityName = weatherData.name;
                int countWeather = 0;
                weatherData.weather.ForEach(W =>
                {
                    weatherInfoDTO.WeatherMain += W.main;
                    weatherInfoDTO.WeatherDescription += W.description;
                    if(countWeather < weatherData.weather.Count() - 1)
                    {
                        weatherInfoDTO.WeatherMain += " ";
                        weatherInfoDTO.WeatherDescription += " ";
                    }
                    countWeather++;
                });

                weatherInfoDTO.WeatherIcon = "http://openweathermap.org/img/wn/04d@2x.png";
                weatherInfoDTO.MainTemp = (int)weatherData.main.temp;
                weatherInfoDTO.MainHumidity = (int)weatherData.main.humidity;

                if (weatherInfoDTO != null)
                {
                    weatherDTO.Data.Add(weatherInfoDTO);
                }
            });

            return weatherDTO;
        }
    }
}
