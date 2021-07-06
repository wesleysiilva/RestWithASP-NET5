using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestWithASPNET.Model.Context;
using RestWithASPNET.Business;
using RestWithASPNET.Business.Implementations;
using RestWithASPNET.Repository;
using Serilog;
using System;
using System.Collections.Generic;
using RestWithASPNET.Repository.Generic;
using Microsoft.Net.Http.Headers;

namespace RestWithASPNET {
  public class Startup {
    public IWebHostEnvironment Enviroment { get; }
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration, IWebHostEnvironment enviroment) {
      Configuration = configuration;
      Enviroment = enviroment;
      Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {

      services.AddControllers();

      var connection = Configuration["MySQLConnection:MySQLConnectionString"];
      services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

      if (Enviroment.IsDevelopment()) {
        MigrateDataBase(connection);
      }

      //Passa a receber e enviar json ou xml, de acordo com a propriedade accept no header da requisição
      services.AddMvc(options => {
        options.RespectBrowserAcceptHeader = true; 
        options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
        options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
      }).AddXmlSerializerFormatters();

      //Versionamento das APIs
      services.AddApiVersioning();

      //Injeção de dependencia, referencia a interface e a implementação da API.
      //=======================================================================
      //Person
      services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();

      //Books
      services.AddScoped<IBookBusiness, BookBusinessImplementation>();

      //Generic
      services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
      //=======================================================================
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }

    private void MigrateDataBase(string connection) {
      try {
        var evolveConnection = new MySql.Data.MySqlClient.MySqlConnection(connection);
        var evolve = new Evolve.Evolve(evolveConnection, msg => Log.Information(msg)) {
          Locations = new List<string> { "db/migrations", "db/dataset" },
          IsEraseDisabled = true,
        };
        evolve.Migrate();
      } catch (Exception ex) {
        Log.Error("Database migration failed", ex);
        throw;
      }
    }
  }
}
