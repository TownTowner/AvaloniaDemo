using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaSix.ViewModels;

namespace AvaloniaSix.Views;

public partial class ActionTabPrintView : UserControl
{
    public ActionTabPrintView()
    {
        InitializeComponent();
    }

    private void PrintList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var item = e.AddedItems.Count > 0 ? e.AddedItems[0] : null;
        if (item is ActionTabPrintViewModel vm)
        {
            if (vm.IsNewItem)
            {
                JobNameTxt.SelectAll();
                JobNameTxt.Focus();
            }
        }
    }
}