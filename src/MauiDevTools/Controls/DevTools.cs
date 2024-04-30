using Microsoft.Maui.Platform;

namespace MauiDevTools.Controls
{
    public class DevTools : Grid, IDisposable
    {
        DevToolsLauncher _launcher;
        DevToolsDashboard _dashboard;
        Profiler _profiler;
        Stack<View> _tools;
        TapGestureRecognizer _tapGestureRecognizer; 
        bool _debuggingPaint;

        public DevTools()
        {
            BackgroundColor = Colors.Transparent;
            InputTransparent = false;

            _tools = new Stack<View>();

            _launcher = new DevToolsLauncher
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(12)
            };
            _launcher.Opened += OpenDashboard;
            Children.Add(_launcher);

            _tapGestureRecognizer = new TapGestureRecognizer();
            _tapGestureRecognizer.Tapped += Tapped;
            GestureRecognizers.Add(_tapGestureRecognizer);
        }

        void Tapped(object? sender, TappedEventArgs e)
        {
            for (int i = 0; i < _tools.Count; i++)
            {
                var child = _tools.Pop();
                Children.Remove(child);
            }
        }

        public void Dispose()
        {
            GestureRecognizers.Clear();
        }

        void OpenDashboard(object? sender, EventArgs e)
        {
            if (_dashboard is null)
            {
                _dashboard = new DevToolsDashboard
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Start
                };

                _dashboard.PerfOverlay += OnPerfOverlay;
                _dashboard.DebugPaint += OnDebugPaint;
            }

            Children.Add(_dashboard);
            _tools.Push(_dashboard);
        }

        void OnPerfOverlay(object? sender, EventArgs e)
        {
            if (_profiler is null)
            {
                _profiler = new Profiler
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                    Margin = new Thickness(12)
                };
            }

            var child = _tools.Pop();
            Children.Remove(child);

            Children.Add(_profiler);
            _tools.Push(_profiler);
        }

        void OnDebugPaint(object? sender, EventArgs e)
        {
            var child = _tools.Pop();
            Children.Remove(child);

            var random = new Random();

            if (!_debuggingPaint)
            {
                Microsoft.Maui.Handlers.ElementHandler.ElementMapper.AppendToMapping("DebugPaint", (handler, view) =>
                {
#if ANDROID
                if (handler.PlatformView is Android.Views.View aView)
                {     
                    aView.Background = null;
                    var color =Color.FromRgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));     
                    aView.SetBackgroundColor(color.ToPlatform());
                }
#elif WINDOWS
                    if (handler.PlatformView is Microsoft.UI.Xaml.Controls.Control control)
                    {
                        control.Background = null;
                        var color = Color.FromRgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                        control.Background = color.ToPlatform();
                    }
#endif
                });

                _debuggingPaint = true;
            }
        }
    }
}