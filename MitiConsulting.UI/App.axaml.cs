using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MitiConsulting.ApplicationCore.Interfaces;
using MitiConsulting.ApplicationCore.Services;
using MitiConsulting.Application.Profil;
using MitiConsulting.UI.Views;
using MitiConsulting.UI.Fiches.ViewModels;
using MitiConsulting.UI.Main;
using MitiConsulting.Infrastructure;
using AutoMapper;


namespace MitiConsulting.UI
{
    public partial class App : Avalonia.Application
    {
        public static IServiceProvider? Services { get; private set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var services = new ServiceCollection();

  
            string connectionString =
                "Server=localhost,1433;Database=MitiDB;User Id=sa;Password=YourPassword123;TrustServerCertificate=True;";

 
            services.AddInfrastructure(connectionString);

            services.AddScoped<IRapportService, RapportService>();

            // AutoMapper
            services.AddAutoMapper(typeof(RapportProfile).Assembly);

 
            services.AddSingleton<MainViewModel>();
            services.AddTransient<RapportsListViewModel>();
            services.AddTransient<RapportFormViewModel>();

            // Build DI container
            Services = services.BuildServiceProvider();


            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = Services.GetRequiredService<MainViewModel>()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
        
    }
}
