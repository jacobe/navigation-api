using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NavigationApi.Api;
using NavigationApi.Api.Controllers;
using NavigationApi.Api.Domain;
using NavigationApi.Test.Persistance.CosmosDb;
using Xunit;

namespace NavigationApi.Test.Controllers
{
    public class MapsControllerTest
    {
        [Fact]
        public async Task POST_creates_a_map()
        {
            var mapRepositoryMock = new Mock<IMapRepository>();
            mapRepositoryMock
                .Setup(m => m.Create(It.IsAny<Map>()))
                .Returns(Task.FromResult(new Map("map1")))
                .Verifiable();

            var hostBuilder = new WebHostBuilder()
                .ConfigureServices(svc =>
                {
                    svc.AddSingleton<IMapRepository>(mapRepositoryMock.Object);
                    svc.AddMvc();
                })
                .Configure(app => app.UseMvc());

            using (var server = new TestServer(hostBuilder))
            {
                var response = await server.CreateRequest("/maps")
                    .And(msg => msg.Content = new StringContent("{\"id\":\"map1\",\"nodes\":{\"a\":{}}}", Encoding.UTF8, "application/json"))
                    .PostAsync();

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("/maps/map1", response.Headers.Location.ToString());
                mapRepositoryMock.Verify(m => m.Create(It.Is<Map>(map => map.Id == "map1" && map.Nodes.Single().Key == "a")));
            }
        }
    }
}