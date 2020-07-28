using Elastic.Apm.NetCoreAll;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Session;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace API2
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
            services.AddControllers();

            var store = new DocumentStore
            {
                Urls = new string[] { "http://raven:8080" },
                Database = "starships"
            };

            store.Initialize();

            services.AddSingleton<IDocumentStore>(store);

            services.AddScoped(serviceProvider =>
            {
                return serviceProvider
                    .GetService<IDocumentStore>()
                    .OpenAsyncSession();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API2", Version = "v1" });
            });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAllElasticApm(Configuration);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            InitialData(app).GetAwaiter().GetResult();
        }

        public async Task InitialData(IApplicationBuilder app)
        {

            var starships = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Starship>>(File.ReadAllText("InitialData.json"));
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var store = serviceScope.ServiceProvider.GetRequiredService<IDocumentStore>();
                EnsureDatabaseExists(store, "starships");

                var db = serviceScope.ServiceProvider.GetRequiredService<IAsyncDocumentSession>();

                if (!await db.Query<Starship>().AnyAsync())
                {
                    foreach (var item in starships)
                    {
                        await db.StoreAsync(item);
                    }

                    await db.SaveChangesAsync();
                }
            }
        }

        public void EnsureDatabaseExists(IDocumentStore store, string database = null, bool createDatabaseIfNotExists = true)
        {
            database = database ?? store.Database;

            if (string.IsNullOrWhiteSpace(database))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(database));

            try
            {
                store.Maintenance.ForDatabase(database).Send(new GetStatisticsOperation());
            }
            catch (DatabaseDoesNotExistException)
            {
                if (createDatabaseIfNotExists == false)
                    throw;

                store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(database)));
            }
        }
    }
}
