namespace MauiDevTools.Controls;

public partial class DevToolsDashboard : ContentView
{
	public DevToolsDashboard()
	{
		InitializeComponent();
	}

    public event EventHandler? PerfOverlay;
    public event EventHandler? DebugPaint;

    void PerfOverlayTapped(object sender, TappedEventArgs e)
    {
        PerfOverlay?.Invoke(this, EventArgs.Empty);
    }

    void DebugPaintTapped(object sender, TappedEventArgs e)
    {
        DebugPaint?.Invoke(this, EventArgs.Empty);
    }
}