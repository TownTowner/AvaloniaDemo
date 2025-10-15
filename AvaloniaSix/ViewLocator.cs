using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaSix.ViewModels;
using System;

namespace AvaloniaSix;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        var viewName = param.GetType().FullName!.Replace("ViewModel", "View");
        var viewType = Type.GetType(viewName);
        if (viewType is null)
            return null;

        var control = Activator.CreateInstance(viewType) as Control;
        control!.DataContext = param;
        return control;
    }

    public bool Match(object? data) => data is PageViewModel;
}
