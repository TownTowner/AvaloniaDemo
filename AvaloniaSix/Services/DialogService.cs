using AvaloniaSix.Interfaces;
using AvaloniaSix.ViewModels;
using System.Threading.Tasks;

namespace AvaloniaSix.Services;

public class DialogService
{
    public async Task ShowDialogAsync<TProvider, TDialog>(TProvider provider, TDialog dialog)
        where TProvider : IDialogProvider
        where TDialog : DialogViewModel
    {
        provider.Dialog = dialog;
        dialog.Show();
        await dialog.WaitAsync();
    }
}
