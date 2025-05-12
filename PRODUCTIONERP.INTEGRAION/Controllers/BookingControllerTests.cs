using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PRODUCTIONERP.INTEGRAION.Controllers
{
    public class BookingControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public BookingControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetTerrorBookingsInRange_ShouldReturnSuccess()
        {
            var fechaInicio = "2025-01-01";
            var fechaFin = "2025-12-31";
            var url = $"/api/bookings/terror?fechaInicio={fechaInicio}&fechaFin={fechaFin}";

            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<RespuestaAPI>(json);

            Assert.True(apiResponse.IsSuccess);
            Assert.NotNull(apiResponse.Result);
        }

        public class RespuestaAPI
        {
            public bool IsSuccess { get; set; }
            public object Result { get; set; }
            public List<string> ErrorMessages { get; set; }
        }
    }
}
