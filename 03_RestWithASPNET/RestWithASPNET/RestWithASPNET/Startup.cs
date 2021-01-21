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
using RestWithASPNET.Repository.Implementations;
using Serilog;
using System;
using System.Collections.Generic;

namespace RestWithASPNET {
  public class Startup {

    public IWebHostEnvironment Environment { get; }
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration, IWebHostEnvironment environment) {
      this.Configuration = configuration;
      this.Environment = environment;

      Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {

      services.AddControllers();

      var connection = Configuration["MySQLConnection:MySQLConnectionString"];
      services.AddDbContext<MySQLContext>(options => options.UseMySql(connection));

      if (Environment.IsDevelopment()) {
        MigrateDatabase(connection);
      }

      services.AddApiVersioning();

      //Dependecy injection
      services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();
      services.AddScoped<IPersonRepository, PersonRepositoryImplementation>();

      services.AddScoped<IBookBusiness, BookBusinessImplementation>();
      services.AddScoped<IBookRepository, BookRepositoryImplementation>();
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

    private void MigrateDatabase(string connection) {
      try {
        var evolveConnetion = new MySql.Data.MySqlClient.MySqlConnection(connection);
        var evolve = new Evolve.Evolve(evolveConnetion, msg => Log.Information(msg)) {
          Locations = new List<string> { "db/migrations", "db/dataset" },
          IsEraseDisabled = true,
        };

        evolve.Migrate();

      } catch (Exception ex) {
        Log.Error("DataBase migration failed", ex);
        throw;
      }
    }

  }
}
