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
}