using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaSix.Data;
using AvaloniaSix.ViewModels;
using System;

namespace AvaloniaSix.Views;

public partial class ActionsPageView : UserControl
{
    public ActionsPageView()
    {
        InitializeComponent();
    }

    private void ActionTab_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (!Equals(e.Source, ActionsTabControl))
            return;

        OnTabChanged();
    }

    protected override void OnInitialized()
    {
        OnTabChanged();

        base.OnInitialized();
    }

    private void OnTabChanged()
    {
        var selectedPage = (ActionsTabControl?.SelectedItem as TabItem)?.Content as Control;
        if (selectedPage is null)
            return;

        var actionTabName = selectedPage switch
        {
            ActionPrintView => ActionsTabName.Print,
            _ => ActionsTabName.Print
        };

        var vm = selectedPage?.DataContext as ActionsPageViewModel;
        if (vm is null)
            return;

        vm?.RefreshPrintList(actionTabName);
    }

}