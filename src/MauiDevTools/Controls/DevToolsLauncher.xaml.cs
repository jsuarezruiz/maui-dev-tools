using System.Windows.Input;

namespace MauiDevTools.Controls;

public partial class DevToolsLauncher : ContentView
{
	public DevToolsLauncher()
	{
		InitializeComponent();
	}

    public event EventHandler? Opened;

    public static readonly BindableProperty CommandProperty =
         BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(DevToolsLauncher), null);

    public ICommand Command
    {
        get { return (ICommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(DevToolsLauncher), null);

    public object CommandParameter
    {
        get { return (object)GetValue(CommandParameterProperty); }
        set { SetValue(CommandParameterProperty, value); }
    }

    void Tapped(object sender, TappedEventArgs e)
    {
        Opened?.Invoke(this, EventArgs.Empty);

        if (Command is not null && Command.CanExecute(CommandParameter))
            Command.Execute(CommandParameter);
    }
}