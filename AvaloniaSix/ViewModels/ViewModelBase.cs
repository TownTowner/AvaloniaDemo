using Avalonia.Controls;
using AvaloniaSix.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AvaloniaSix.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    protected JsonSerializerOptions _jsonOptions = new()
    {
        IgnoreReadOnlyFields = false,
        IgnoreReadOnlyProperties = false,
        WriteIndented = true,
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
    };

    public string SavedState = "";

    [JsonIgnore]
    public virtual bool HasChanged
    {
        get
        {
            var t = GetType().DeclaringType ?? GetType();
            return SavedState != "" && SavedState != JsonSerializer.Serialize(this, t, _jsonOptions);
        }
    }

    public ViewModelBase()
    {
        if (Design.IsDesignMode)
        {
            OnDesignConstructor();
        }
    }

    protected virtual void OnDesignConstructor() { }

    public void SetSaveState()
    {
        SavedState = GetSaveState();

        OnPropertyChanged(nameof(HasChanged));
    }

    public string GetSaveState()
    {
        var t = GetType().DeclaringType ?? GetType();
        return JsonSerializer.Serialize(this, t, _jsonOptions);
    }

    [RelayCommand]
    public void RestoreState(string? stateJson = null)
    {
        stateJson ??= SavedState;

        var t = GetType().DeclaringType ?? GetType();
        var store = JsonSerializer.Deserialize(stateJson, t, _jsonOptions);

        var properties = t.GetProperties()
            .Where(x => x.CanWrite && x.GetCustomAttribute<JsonIgnoreAttribute>() == null);
        foreach (var item in properties)
        {
            var value = item.GetValue(store);
            item.SetValue(this, value);
        }
    }
}

public partial class PageViewModel : ViewModelBase
{
    [ObservableProperty]
    public ApplicationPageName _pageName;

    public PageViewModel(ApplicationPageName pageName)
    {
        _pageName = pageName;
    }

}