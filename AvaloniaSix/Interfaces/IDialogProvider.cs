using AvaloniaSix.ViewModels;

namespace AvaloniaSix.Interfaces;

public interface IDialogProvider
{
    DialogViewModel Dialog { get; set; }
}
