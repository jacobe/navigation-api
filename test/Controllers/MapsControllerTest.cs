using System.Net;
using System.Net.Http;
using System.Text;
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
                var response = await server.CreateRequest("/maps")
                    .And(msg => msg.Content = new StringContent("{\"id\":\"map1\"}", Encoding.UTF8, "application/json"))
                    .PostAsync();

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("/maps/map1", response.Headers.Location.ToString());
            }
        }
    }
}