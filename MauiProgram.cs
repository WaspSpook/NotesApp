using Microsoft.Extensions.Logging;
using NotesApp.Services;
using NotesApp.ViewModels;
using NotesApp.Views;
using System;
using System.IO;

#if WINDOWS
using Microsoft.Windows.ApplicationModel.WindowsAppRuntime;
#endif

namespace NotesApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
#if WINDOWS
        // Инициализация Windows App Runtime (только для Windows)
        try
        {
            DeploymentManager.Initialize();
        }
        catch (Exception ex)
        {
            // Запись ошибки для диагностики (путь можно изменить при необходимости)
            string errorPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "deployment_error.txt");
            File.WriteAllText(errorPath, ex.ToString());
        }
#endif

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Регистрация базы данных
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "notes.db3");
        builder.Services.AddSingleton(new DatabaseService(dbPath));

        // Регистрация ViewModels и страниц
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<NotePage>();
        builder.Services.AddTransient<AppShell>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}