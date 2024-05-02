using MauiDevTools.Controls;

namespace MauiDevTools.Gallery
{
    public partial class App : DevToolsApplication
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
