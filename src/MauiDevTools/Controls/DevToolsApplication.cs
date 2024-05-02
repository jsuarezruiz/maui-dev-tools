namespace MauiDevTools.Controls
{
    public class DevToolsApplication : Application
    {
        DevToolsOverlay? _devToolsOverlay;

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);
            
            _devToolsOverlay = new DevToolsOverlay(window);

            window.Activated += (sender, args) =>
            {
                window.AddOverlay(_devToolsOverlay);
            };

            return window;
        }
    }
}
