using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using AvaloniaSix.Data;
using AvaloniaSix.Factories;
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

        collections.AddSingleton<Func<ApplicationPageName, PageViewModel>>(x => name => name switch
        {
            ApplicationPageName.Actions => x.GetRequiredService<ActionsPageViewModel>(),
            ApplicationPageName.History => x.GetRequiredService<HistoryPageViewModel>(),
            ApplicationPageName.Home => x.GetRequiredService<HomePageViewModel>(),
            ApplicationPageName.Macros => x.GetRequiredService<MacrosPageViewModel>(),
            ApplicationPageName.Process => x.GetRequiredService<ProcessPageViewModel>(),
            ApplicationPageName.Reporter => x.GetRequiredService<ReporterPageViewModel>(),
            ApplicationPageName.Settings => x.GetRequiredService<SettingsPageViewModel>(),
            _ => throw new InvalidOperationException()
        });
        collections.AddSingleton<PageFactory>();

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