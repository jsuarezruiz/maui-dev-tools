using CommunityToolkit.Maui.Views;

namespace MauiDevTools.Controls;

public partial class DevToolsDashboard : Popup
{
	public DevToolsDashboard()
	{
		InitializeComponent();
	}

    public event EventHandler? PerfOverlay;
    public event EventHandler? DebugPaint; 
    public event EventHandler? DebugLogs; 
    public event EventHandler? DeviceInfo;
    public event EventHandler? ColorPicker;

    void PerfOverlayTapped(object sender, EventArgs e)
    {
        var currentPage = Application.Current?.MainPage;

        if (currentPage is Shell shell)
        {
            currentPage = shell.CurrentPage;
        }

        if (currentPage is not null)
        {
            Close();

            Profiler profiler = new Profiler();
            currentPage.ShowPopup(profiler);
        }

        PerfOverlay?.Invoke(this, EventArgs.Empty);
    }

    void DebugPaintTapped(object sender, TappedEventArgs e)
    {
        DebugPaint?.Invoke(this, EventArgs.Empty);
    }
}