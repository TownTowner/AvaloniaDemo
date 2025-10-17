using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaSix.ViewModels;

namespace AvaloniaSix.Views;

public partial class ActionPrintView : UserControl
{
    public ActionPrintView()
    {
        InitializeComponent();
    }

    private void PrintList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var vm = e.AddedItems.Count > 0 ? e.AddedItems[0] : null;
        if (vm is ActionPrintViewModel { IsNewItem: true })
        {
            JobNameTxt.SelectAll();
            JobNameTxt.Focus();
        }
    }
}