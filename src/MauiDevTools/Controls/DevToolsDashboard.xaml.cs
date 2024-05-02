using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Platform;

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
        var random = new Random();

        Microsoft.Maui.Handlers.ElementHandler.ElementMapper.AppendToMapping("DebugPaint", (handler, view) =>
        {
#if ANDROID
             
            if (handler.PlatformView is Android.Views.View aView)
            {     
                aView.Background = null;
                var color = Color.FromRgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));     
                aView.SetBackgroundColor(color.ToPlatform());
            }
#endif
        });

        DebugPaint?.Invoke(this, EventArgs.Empty);
    }
}