using Avalonia.Controls;
using Avalonia.Platform.Storage;
using AvaloniaSix.Interfaces;
using AvaloniaSix.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AvaloniaSix.Services;

public class DialogService(Func<TopLevel?> topLevel)
{
    public async Task ShowDialogAsync<TProvider, TDialog>(TProvider provider, TDialog dialog)
        where TProvider : IDialogProvider
        where TDialog : DialogViewModel
    {
        provider.Dialog = dialog;
        dialog.Show();
        await dialog.WaitAsync();
    }

    public async Task<string> FolderPicker()
    {
        var topLevelVisual = topLevel();
        if (topLevelVisual == null)
            throw new InvalidOperationException("No top-level visual available for folder picker.");

        var folders = await topLevelVisual.StorageProvider.OpenFolderPickerAsync(
            new()
            {
                Title = "Select Folder",
                AllowMultiple = false
            });

        var path = folders.FirstOrDefault()?.Path;
        if (path == null)
            return string.Empty;

        return path.IsAbsoluteUri ? path.LocalPath : path.OriginalString;
    }


}
