using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NavigationApi.Api.Domain;

namespace NavigationApi.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDocumentClient>(svc => 
                new DocumentClient(
                    serviceEndpoint: new Uri(Configuration["CosmosDB:EndpointUrl"]),
                    authKeyOrResourceToken: Configuration["CosmosDB:AuthKey"]));

            services.AddTransient<IMapRepository>(svc =>
                new Persistance.CosmosDb.MapRepository(
                    documentClient: svc.GetService<IDocumentClient>(),
                    databaseId: Configuration["CosmosDB:DatabaseId"]));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
