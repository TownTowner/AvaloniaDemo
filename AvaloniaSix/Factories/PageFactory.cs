using AvaloniaSix.Data;
using AvaloniaSix.ViewModels;
using System;

namespace AvaloniaSix.Factories;

public class PageFactory(Func<ApplicationPageName, PageViewModel> pageFactory)
{
    public PageViewModel CreatePage(ApplicationPageName pageName) => pageFactory(pageName);
}
