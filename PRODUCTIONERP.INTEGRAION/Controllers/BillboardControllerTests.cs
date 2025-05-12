using FERSOFT.ERP.API.Controllers.Response;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Net;

namespace PRODUCTIONERP.INTEGRAION.Controllers
{
    public class BillboardControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public BillboardControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }


        [Fact]
        public async Task GetSeatsStatusTodayAsync_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/billboard/estado-butacas-hoy");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<RespuestaAPI>(json);

            Assert.True(apiResponse.IsSuccess);
            Assert.NotNull(apiResponse.Result);
        }


        [Fact]
        public async Task CancelBillboard_ShouldCancelAndReturnSuccess()
        {
            var billboardId = 1; 
            var response = await _client.DeleteAsync($"/api/billboard/cancelar/{billboardId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<RespuestaAPI>(json);

            Assert.True(apiResponse.IsSuccess);
            Assert.Contains("Clientes afectados", json); 
        }


       
    }
}