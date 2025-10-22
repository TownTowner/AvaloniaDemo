using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AvaloniaSix.ViewModels;

public partial class ConfirmDialogViewModel : DialogViewModel
{
    [ObservableProperty]
    private string _title = "Confirm";

    [ObservableProperty]
    private string _message = "Are you sure?";

    [ObservableProperty]
    private string _statusText = string.Empty;

    [ObservableProperty]
    private string _progressText = string.Empty;

    [ObservableProperty]
    private string _confirmText = "Yes";

    [ObservableProperty]
    private string _cancelText = "No";

    [ObservableProperty]
    private string _iconText = "\xe4e0";

    [ObservableProperty]
    private bool _isConfirmed = false;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CancelCommand))]
    private bool _isBusy = false;

    public bool NotBusy => !IsBusy;

    [ObservableProperty]
    private double _dialogWidth = double.NaN;// equal to Auto

    [ObservableProperty]
    private double _dialogHeight = double.NaN;// equal to Auto

    [JsonIgnore]
    public Func<ConfirmDialogViewModel, Task<bool>>? OnConfirmedAsync { get; set; } = (_) => Task.FromResult(true);

    [RelayCommand]
    public async Task ConfirmAsync()
    {
        if (IsBusy) return;

        IsBusy = true;

        StatusText = string.Empty;
        ProgressText = "Processing...";

        var result = await OnConfirmedAsync?.Invoke(this);

        IsBusy = false;
        if (!result) return;

        IsConfirmed = true;
        Close();
    }

    [RelayCommand(CanExecute = nameof(NotBusy))]
    public Task Cancel()
    {
        IsConfirmed = false;
        Close();
        return Task.CompletedTask;
    }
}
