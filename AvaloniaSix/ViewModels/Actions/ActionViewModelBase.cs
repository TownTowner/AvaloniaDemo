using AvaloniaSix.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace AvaloniaSix.ViewModels;

public partial class ActionViewModelBase : ViewModelBase
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _id = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _jobName = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _description = "";

    [ObservableProperty]
    private bool _isNewItem;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private int _sortOrder;

    [JsonIgnore]
    public override bool HasChanged => IsNewItem || base.HasChanged;

    //public ActionEntityBase ToEntity() => new()
    //{
    //    Id = Id,
    //    Description = Description,
    //    JobName = JobName,
    //    SortOrder = SortOrder
    //};
}
