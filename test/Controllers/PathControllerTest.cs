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
                    svc.AddSingleton<IMapRepository>(mapRepositoryMock.Object);
                    svc.AddTransient<IPathAlgorithm, ShortestPathAlgorithm>();
                    svc.AddMvc();
                })
                .Configure(app => app.UseMvc());

            using (var server = new TestServer(hostBuilder))
            {
                var response = await server.CreateRequest("/maps/map1/path")
                    .GetAsync();

                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }
}