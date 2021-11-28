using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
//using Microsoft.Maui.Essentials;
using Microsoft.Maui.LifecycleEvents;
using System;
using System.Net.Http;
using Syncfusion.Licensing;
using Syncfusion.Blazor;
using Blazored.SessionStorage;
using Notes2022.RCL;
using System.Collections.Generic;
using Notes2022.Shared;

namespace Notes2022Preview
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            string licenseKey = "NTE4MzYzQDMxMzkyZTMzMmUzMFBEQXBJMFcxY0s0T09hVW9HRElOMDU1dHlKT3pVRmNWbGdKUVZtcHlNQWM9;NTE4MzY0QDMxMzkyZTMzMmUzMGo2RW9MSytRaHUxZ1pBalVPd0Y0Zk5XS3ZmcWZuMWpwMTJpbVRTYVBFd1U9;NTE4MzY1QDMxMzkyZTMzMmUzMGlld3RldnBSaFdOeWZxVCtjN0dqb3dmdC9CbTlSeTI4OWJ6ZG52Rk9PNmM9;NTE4MzY2QDMxMzkyZTMzMmUzME8wWElrMXhZS3pOMlMxYU8vN05iNFRRRjNLRjFYbTlZUE51aWNpM1E2b0E9;NTE4MzY3QDMxMzkyZTMzMmUzMGZIYlJCOU5GQm9yODJ4ZnVUbGtmQUNRT2lRbXAvN01CMlVRUXljUTAzMnM9;NTE4MzY4QDMxMzkyZTMzMmUzMEk4aEJaYVA4N3owN2d6dFdjNHNhaElXV1NwQUF5d1NOaXU1bTRsWHlER2s9;NTE4MzY5QDMxMzkyZTMzMmUzMEpKS0JSTkQ2Tm5QU3g0QVFJSWNUL3RraS9hMi9JV00wTE1qaXR4YW9kZ009;NTE4MzcwQDMxMzkyZTMzMmUzMEIzQkMxeEpqcEMxTDlaZ3o3TFE5dnpES3hhajlRMzlocG0vSmhNLzQwZEU9;NTE4MzcxQDMxMzkyZTMzMmUzMFZMOTcvYkt2bEE4NHpBZjZycGIzU1puaXpNSllMOGRDaVhDajlxbW0wTnc9;NTE4MzcyQDMxMzkyZTMzMmUzMFk0d0VTNWsvb09wb08vRXVGTEF1VzlJY2VVK0VRcHY4ZXlGZzA4Y0hFdDQ9;NTE4MzczQDMxMzkyZTMzMmUzME1NYTdNdnN0UWxLdHVvZ2JxZzhyRHhBMTZvcFJvTXFDY3ltYzQzSHd3WEk9 ";
            SyncfusionLicenseProvider.RegisterLicense(licenseKey);


            var builder = MauiApp.CreateBuilder();

#if WINDOWS
			Microsoft.Maui.Essentials.Platform.MapServiceToken =
				"RJHqIE53Onrqons5CNOx~FrDr3XhjDTyEXEjng-CRoA~Aj69MhNManYUKxo6QcwZ0wmXBtyva0zwuHB04rFYAPf7qqGJ5cHb03RCDw1jIW8l";
#endif

            builder
                .ConfigureLifecycleEvents(lifecycle =>
                {
#if __IOS__
					lifecycle
						.AddiOS(iOS => iOS
							.OpenUrl((app, url, options) =>
								Microsoft.Maui.Essentials.Platform.OpenUrl(app, url, options))
							.ContinueUserActivity((application, userActivity, completionHandler) =>
								Microsoft.Maui.Essentials.Platform.ContinueUserActivity(application, userActivity, completionHandler))
							.PerformActionForShortcutItem((application, shortcutItem, completionHandler) =>
								Microsoft.Maui.Essentials.Platform.PerformActionForShortcutItem(application, shortcutItem, completionHandler)));
#elif WINDOWS
					lifecycle
						.AddWindows(windows => windows
							.OnLaunched((app, e) =>
								Microsoft.Maui.Essentials.Platform.OnLaunched(e)));
#endif
                })
                .UseMauiApp<App>();

            builder
                .RegisterBlazorMauiWebView()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
                .Host
                .ConfigureAppConfiguration((app, config) =>
                {
                    string dir = Environment.ProcessPath;
                    int index = dir.LastIndexOf('\\');
                    dir = dir.Substring(0, index);
                    var Provider = new PhysicalFileProvider(dir);
                    config.AddJsonFile(Provider, "appsettings.json", optional: false, false);
                });

            string baseAddress = builder.Configuration["BaseAddress"];

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

            //.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            //builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Notes2022.ServerAPI"));

            builder.Services.AddApiAuthorization();

            builder.Services.AddSyncfusionBlazor();

            builder.Services.AddBlazoredSessionStorage();

            builder.Services.AddBlazorWebView();

            if (Globals.UserDataList is null)
            {
                Globals.UserDataList = new List<UserData>();
                Globals.StartupDateTime = DateTime.Now.ToUniversalTime();
            }

            return builder.Build();
        }
    }
}