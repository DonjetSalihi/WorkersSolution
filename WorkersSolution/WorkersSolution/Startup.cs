using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using WorkersSolution.Constants;
using WorkersSolution.Models;
using WorkersSolution.Services;
using WorkersSolution.ViewModels;
using Xamarin.Essentials;

namespace WorkersSolution
{
    public class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void Init()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("WorkersSolution.appsettings.json");

            //Here we create a builder for adding service (logging etc) onto the app.
            var host = new HostBuilder()
                .ConfigureHostConfiguration(c =>
                {
                    c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                    c.AddJsonStream(stream);
                })
                .ConfigureServices((c, x) => ConfigureServices(c, x))
                .ConfigureLogging(l => l.AddConsole(o =>
                {
                    o.DisableColors = true;
                }))
                .Build();

            //Handles all dependency injection that comes from HostBuilder
            ServiceProvider = host.Services;
        }

        static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            var project = ctx.Configuration["Project"];

            services.AddHttpClient("WorkersApi", client =>
            {
                client.BaseAddress = new Uri(AppConfig.WorkersApiBaseUrl);
            });

            // add as a singleton so only one ever will be created.
            services.AddSingleton<IRepository<Worker>, WorkerService>();

            // add the ViewModel, but as a Transient, which means it will create a new one each time.
            services.AddTransient<WorkersViewModel>();
        }
    }
}
