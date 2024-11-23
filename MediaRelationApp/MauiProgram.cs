using MediaRelationApp.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using WinRT.Interop;

namespace MediaRelationApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<ApiService>();
#if WINDOWS
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(windows =>
                {
                    windows.OnWindowCreated(window =>
                    {
                        var nativeWindow = window as Microsoft.UI.Xaml.Window;
                        if (nativeWindow != null)
                        {
                            // Brug AppWindow til at ændre vinduets størrelse
                            var appWindow = AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(WindowNative.GetWindowHandle(nativeWindow)));

                            if (appWindow != null)
                            {
                                appWindow.Resize(new SizeInt32(1800, 1200));
                                appWindow.Move(new PointInt32(200, 100));
                            }
                        }
                    });
                });
            });
#endif
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
