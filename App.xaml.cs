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
            Routing.RegisterRoute(nameof(NotePage), typeof(NotePage));

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