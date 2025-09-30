namespace AvaloniaDemo.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia! This is my added text.";
    public Person person { get; set; } = new Person
    {
        Name = "John Doe"
    };
}
