using AvaloniaSix.Data;
using AvaloniaSix.ViewModels;
using System;

namespace AvaloniaSix.Factories;

public class PageFactory(Func<Type, PageViewModel> factory)
{
    public PageViewModel CreatePage<T>(Action<T>? afterCreation = null)
        where T : PageViewModel
    {
        var viewModel = factory(typeof(T));

        afterCreation?.Invoke((T)viewModel);

        return viewModel;
    }
}
