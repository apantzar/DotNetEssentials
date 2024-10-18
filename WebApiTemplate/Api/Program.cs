using Api.Configuration;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Debugging;
using System.Reflection;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            try
            {
                Log.Information("Starting web api");

                var builder = WebApplication.CreateBuilder(args);

                var serilogConfiguration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .Build();

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(serilogConfiguration)
                    .CreateLogger();

                #region Services

                builder.Services.AddSerilog();
                SelfLog.Enable(Console.Error);

                var appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
                if (appSettings == null)    throw new Exception("AppSettings not found");

                builder.Services.AddSingleton(appSettings);

                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();

                builder.Services.AddSwaggerGen(o =>
                {
                    o.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = appSettings?.ProjectInfo.Name ?? "Default",
                        Version = appSettings?.ProjectInfo.Version ?? "Default",
                        Description = appSettings?.ProjectInfo.Description ?? "Default",
                        Contact = new OpenApiContact
                        {
                            Name = appSettings?.ProjectInfo.Contact.Name ?? "Default",
                            Email = appSettings?.ProjectInfo.Contact.Email ?? string.Empty,
                            Url = new Uri(appSettings?.ProjectInfo.Contact.Url ?? string.Empty)
                        }
                    });

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    if (File.Exists(xmlPath))
                    {
                        o.IncludeXmlComments(xmlPath);
                    }
                });

                #endregion

                var app = builder.Build();

                app.UseSerilogRequestLogging();

                if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(o =>
                    {
                        o.SwaggerEndpoint("/swagger/v1/swagger.json", $"{appSettings?.ProjectInfo.Name ?? string.Empty} API V1");
                        o.DocumentTitle = $"{appSettings?.ProjectInfo.Name ?? string.Empty} API Documentation";
                    });

                }

                app.UseHttpsRedirection();

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
