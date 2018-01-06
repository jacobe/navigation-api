using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NavigationApi.Api;
using NavigationApi.Api.Controllers;
using Xunit;

namespace NavigationApi.Test.Controllers
{
    public class MapsControllerTest
    {
        [Fact]
        public async Task POST_creates_a_map()
        {
            var hostBuilder = new WebHostBuilder()
                .UseStartup<Startup>();

            using (var server = new TestServer(hostBuilder))
            {
                var response = await server
                    .CreateRequest("/maps")
                    .PostAsync();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}