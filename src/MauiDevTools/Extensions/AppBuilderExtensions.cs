using CommunityToolkit.Maui;
using MauiDevTools.Controls;

namespace MauiDevTools.Extensions
{
    public static class AppBuilderExtensions
    {
        public static MauiAppBuilder UseMauiDevTools(this MauiAppBuilder builder)
        {
            builder.UseMauiCommunityToolkit();

            AddDevToolsOverlay();

            return builder;
        }

        static void AddDevToolsOverlay()
        {
            var application = Application.Current;

            if (application is not null)
            {
                var window = application.GetVisualElementWindow();

                if (window is not null)
                {
                    DevToolsOverlay devToolsOverlay = new DevToolsOverlay(window);
                    window.AddOverlay(devToolsOverlay);
                }
            }
        }
    }
}
