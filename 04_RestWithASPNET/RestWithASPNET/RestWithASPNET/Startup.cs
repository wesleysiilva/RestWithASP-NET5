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
using RestWithASPNET.Hypermedia.Filters;
using RestWithASPNET.Hypermedia.Enricher;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Rewrite;
using RestWithASPNET.Services;
using RestWithASPNET.Services.Implementations;
using RestWithASPNET.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;

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

      //Autenticação
      var tokenConfigurations = new TokenConfiguration();
      new ConfigureFromConfigurationOptions<TokenConfiguration>(
        Configuration.GetSection("TokenConfigurations")
      ).Configure(tokenConfigurations);

      services.AddSingleton(tokenConfigurations);
      services.AddAuthentication(options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

      }).AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = tokenConfigurations.Issuer,
          ValidAudience = tokenConfigurations.Audience,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
        };
      });

      services.AddAuthorization(auth => {
        auth.AddPolicy(
          "Bearer", 
          new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build()
        );
      });
      //=-=-=-=-==-=-=-=-==-=-=-=-==-=-=-=-==-=-=-=-=

      services.AddCors(options => options.AddDefaultPolicy(builder => {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
      }));

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

      var filterOptions = new HyperMediaFilterOptions();
      filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
      filterOptions.ContentResponseEnricherList.Add(new BookEnricher());
      services.AddSingleton(filterOptions);

      //Versionamento das APIs
      services.AddApiVersioning();

      //Suporte ao Swagger
      services.AddSwaggerGen(c => {
        c.SwaggerDoc(
          "v1", 
          new OpenApiInfo{
            Title = "REST API's from 0 to Azure with ASP.NET Core 5 and Docker",
            Version = "v1",
            Description = "API RESTful developed in course 'REST API's from 0 to Azure with ASP.NET Core 5 and Docker'",
            Contact = new OpenApiContact {
              Name = "Wesley Silva",
              Url = new Uri("https://github.com/wesleysiilva")
            }
          });
      });

      //Injeção de dependencia, referencia a interface e a implementação da API.
      //=======================================================================

      //Upload/Download files
      services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddScoped<IFileBusiness, FileBusinessImplementation>();

      //Person
      services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();

      //Books
      services.AddScoped<IBookBusiness, BookBusinessImplementation>();

      //Token
      services.AddScoped<ILoginBusiness, LoginBusinessImplementation>();
      services.AddTransient<ITokenService, TokenService>();
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IPersonRepository, PersonRepository>();

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

      //Configuração do CORS deve ficar depois de UseHttpsRedirection e UseRouting e antes de UseEndpoints
      app.UseCors();

      //Suporte ao swagger
      app.UseSwagger(); //Responsável pelo json com a documentação
      app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "REST API's from 0 to Azure with ASP.NET Core 5 and Docker - v1");
      }); //Responsável pela página html

      var option = new RewriteOptions();
      option.AddRedirect("^$","swagger");
      app.UseRewriter(option);

      app.UseAuthorization();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
        endpoints.MapControllerRoute("DefaultApi", "{controller=values}/{id?}");
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
