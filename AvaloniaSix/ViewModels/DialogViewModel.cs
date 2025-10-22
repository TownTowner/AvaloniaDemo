using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaSix.ViewModels;

public partial class DialogViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isDialogOpen;

    protected TaskCompletionSource closeTask = new();

    public async Task WaitAsync()
    {
        await closeTask.Task;
    }

    public void Show()
    {
        if (closeTask.Task.IsCompleted)
        {
            closeTask = new TaskCompletionSource();
        }

        IsDialogOpen = true;
    }

    public void Close()
    {
        IsDialogOpen = false;
        closeTask.TrySetResult();
    }
}
