using NotesApp.Views;
using Microsoft.Maui.Controls;
using System;
using System.IO;

namespace NotesApp;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        try
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
			    // Устанавливаем культуру на основе языка устройства
			var culture = new System.Globalization.CultureInfo(Android.App.Application.Context.Resources.Configuration.Locales.Get(0).ToString());
			System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
			System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;
			Routing.RegisterRoute(nameof(NotePage), typeof(NotePage));
			
            //Routing.RegisterRoute(nameof(NotePage), typeof(NotePage));

            // Глобальный обработчик непойманных исключений
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, "crash.txt"),
                    $"Unhandled exception: {e.ExceptionObject}");
            };
        }
        catch (Exception ex)
        {
            File.WriteAllText("app_init_error.txt", ex.ToString());
            throw;
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        try
        {
            var shell = _serviceProvider.GetRequiredService<AppShell>();
            return new Window(shell);
        }
        catch (Exception ex)
        {
            File.WriteAllText("window_creation_error.txt", ex.ToString());
            throw;
        }
    }
}