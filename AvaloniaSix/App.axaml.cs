using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using AvaloniaSix.Entities;
using AvaloniaSix.Factories;
using AvaloniaSix.Services;
using AvaloniaSix.ViewModels;
using AvaloniaSix.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: XmlnsDefinition("https://github.com/avaloniaui", "AvaloniaSix.Controls")]

namespace AvaloniaSix;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var collections = new ServiceCollection();
        collections.AddSingleton<MainViewModel>();
        collections.AddTransient<ActionsPageViewModel>();
        collections.AddTransient<HistoryPageViewModel>();
        collections.AddTransient<HomePageViewModel>();
        collections.AddTransient<MacrosPageViewModel>();
        collections.AddTransient<ProcessPageViewModel>();
        collections.AddTransient<ReporterPageViewModel>();
        collections.AddTransient<SettingsPageViewModel>();

        collections.AddSingleton<Func<Type, PageViewModel>>(x => type => type switch
        {
            _ when type == typeof(HomePageViewModel) => x.GetRequiredService<HomePageViewModel>(),
            _ when type == typeof(ProcessPageViewModel) => x.GetRequiredService<ProcessPageViewModel>(),
            _ when type == typeof(MacrosPageViewModel) => x.GetRequiredService<MacrosPageViewModel>(),
            _ when type == typeof(ActionsPageViewModel) => x.GetRequiredService<ActionsPageViewModel>(),
            _ when type == typeof(ReporterPageViewModel) => x.GetRequiredService<ReporterPageViewModel>(),
            _ when type == typeof(HistoryPageViewModel) => x.GetRequiredService<HistoryPageViewModel>(),
            _ when type == typeof(SettingsPageViewModel) => x.GetRequiredService<SettingsPageViewModel>(),
            _ => throw new InvalidOperationException($"Page of type {type?.FullName} has no view model"),
        });
        collections.AddSingleton<PageFactory>();
        collections.AddSingleton<PrinterService>();
        collections.AddSingleton<Func<TopLevel?>>(x =>
        {
            return () =>
            {
                if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime topWindow)
                {
                    return TopLevel.GetTopLevel(topWindow.MainWindow);
                }
                else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
                {
                    return TopLevel.GetTopLevel(singleViewPlatform.MainView);
                }

                throw new InvalidOperationException("No valid TopLevel found for the current application lifetime.");
            };
        });
        collections.AddSingleton<DialogService>();

        //db
        collections.AddTransient<AvaSixDbContext>();
        collections.AddTransient<DbService>();
        collections.AddSingleton<Func<DbService>>(x => x.GetRequiredService<DbService>);
        collections.AddSingleton<DbFactory>();


        var services = collections.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow()
            {
                DataContext = services.GetRequiredService<MainViewModel>()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = services.GetRequiredService<MainViewModel>()
            };
        }


        base.OnFrameworkInitializationCompleted();
    }
}