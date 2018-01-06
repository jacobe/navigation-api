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
using Newtonsoft.Json.Linq;
using Xunit;

namespace NavigationApi.Test.Controllers
{
    public class PathControllerTest
    {
        [Fact]
        public async Task GET_returns_NotFound_when_map_is_not_found()
        {
            var mapRepositoryMock = new Mock<IMapRepository>();
            mapRepositoryMock
                .Setup(m => m.GetById(It.IsAny<string>()))
                .Returns(Task.FromResult<Map>(null))
                .Verifiable();

            var hostBuilder = new WebHostBuilder()
                .ConfigureServices(svc =>
                {
                    svc.AddSingleton<IMapRepository>(mapRepositoryMock.Object)
                       .AddSingleton<IPathAlgorithm>(new Mock<IPathAlgorithm>().Object);
                    svc.AddMvc();
                })
                .Configure(app => app.UseMvc());

            using (var server = new TestServer(hostBuilder))
            {
                var response = await server.CreateRequest("/maps/map1/path?start=a&end=b")
                    .GetAsync();

                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task GET_returns_the_path()
        {
            var mapRepositoryMock = new Mock<IMapRepository>();
            mapRepositoryMock
                .Setup(m => m.GetById(It.IsAny<string>()))
                .Returns(Task.FromResult<Map>(new Map("map1")))
                .Verifiable();

            var pathAlgorithmMock = new Mock<IPathAlgorithm>();
            pathAlgorithmMock
                .Setup(m => m.Find(It.IsAny<Map>(), "a", "b"))
                .Returns(new Path(new[] { "a", "b" }, 5))
                .Verifiable();

            var hostBuilder = new WebHostBuilder()
                .ConfigureServices(svc =>
                {
                    svc.AddSingleton<IMapRepository>(mapRepositoryMock.Object)
                       .AddSingleton<IPathAlgorithm>(pathAlgorithmMock.Object);
                    svc.AddMvc();
                })
                .Configure(app => app.UseMvc());

            using (var server = new TestServer(hostBuilder))
            {
                var response = await server.CreateRequest("/maps/map1/path?start=a&end=b")
                    .GetAsync();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var result = JToken.Parse(await response.Content.ReadAsStringAsync());
                Assert.Equal(5, result["totalDistance"]);
                Assert.Equal("a", result["path"].ElementAt(0).ToString());
                Assert.Equal("b", result["path"].ElementAt(1).ToString());
            }
        }
    }
}