using Avalonia.Controls;
using AvaloniaSix.Data;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaSix.ViewModels;

public class ViewModelBase : ObservableObject
{
}

public partial class PageViewModel : ViewModelBase
{
    [ObservableProperty]
    public ApplicationPageName _pageName;

    public PageViewModel(ApplicationPageName pageName)
    {
        _pageName = pageName;

        if (Design.IsDesignMode)
        {
            OnDesignConstructor();
        }
    }

    protected virtual void OnDesignConstructor() { }
}