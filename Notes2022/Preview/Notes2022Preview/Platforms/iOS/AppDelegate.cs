using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Notes2022Preview
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}