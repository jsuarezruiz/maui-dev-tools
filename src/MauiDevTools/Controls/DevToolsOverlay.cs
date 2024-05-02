using CommunityToolkit.Maui.Views;

namespace MauiDevTools.Controls
{
    public class DevToolsOverlay : WindowOverlay
    {
        DevToolsElement _devToolsElement;
     
        public DevToolsOverlay(IWindow window) : base(window)
        {
            _devToolsElement = new DevToolsElement(this);
            AddWindowElement(_devToolsElement);

            Tapped += OnDevToolsOverlayTapped;
        }

        public event EventHandler? Opened;

        void OnDevToolsOverlayTapped(object? sender, WindowOverlayTappedEventArgs e)
        {
            var tapped = _devToolsElement.Contains(e.Point);

            if (tapped)
            {
                var currentPage = Application.Current?.MainPage;

                if(currentPage is Shell shell)
                {
                    currentPage = shell.CurrentPage;
                }

                if (currentPage is not null)
                {
                    DevToolsDashboard dashboard = new DevToolsDashboard();
                    currentPage.ShowPopup(dashboard);
                }

                Opened?.Invoke(this, EventArgs.Empty);
            }

            Invalidate();
        }

        class DevToolsElement : IWindowOverlayElement
        {
            RectF _boundingBox;
            readonly WindowOverlay _overlay;

            public DevToolsElement(WindowOverlay overlay)
            {
                _overlay = overlay; 
            }

#if ANDROID
            public bool Contains(Point point)
            {
                // TODO: .NET MAUI Bug
                float statusBarHeight = 48;
                return _boundingBox.Contains(new Point(point.X, point.Y - statusBarHeight));
            }
#else       
            public bool Contains(Point point) =>
                _boundingBox.Contains(new Point(point.X, point.Y));
#endif
            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
                canvas.FillColor = Colors.Red;

                float size = 24;

                float x = dirtyRect.Width - (size * 2);
                float y = dirtyRect.Y + size;
                float height = size;
                float width = size;

                _boundingBox = new RectF(x, y, height, width);
                canvas.FillEllipse(_boundingBox);
            }
        }
    }
}
