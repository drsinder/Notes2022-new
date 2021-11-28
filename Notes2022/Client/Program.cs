// Client side

using Blazored.Modal;
using Blazored.SessionStorage;
//using Grpc.Net.Client;
//using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Notes2022.Client;
using Notes2022.RCL;
using Notes2022.Shared;
using Syncfusion.Blazor;
using Syncfusion.Licensing;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

string licenseKey = "NTE4MzYzQDMxMzkyZTMzMmUzMFBEQXBJMFcxY0s0T09hVW9HRElOMDU1dHlKT3pVRmNWbGdKUVZtcHlNQWM9;NTE4MzY0QDMxMzkyZTMzMmUzMGo2RW9MSytRaHUxZ1pBalVPd0Y0Zk5XS3ZmcWZuMWpwMTJpbVRTYVBFd1U9;NTE4MzY1QDMxMzkyZTMzMmUzMGlld3RldnBSaFdOeWZxVCtjN0dqb3dmdC9CbTlSeTI4OWJ6ZG52Rk9PNmM9;NTE4MzY2QDMxMzkyZTMzMmUzME8wWElrMXhZS3pOMlMxYU8vN05iNFRRRjNLRjFYbTlZUE51aWNpM1E2b0E9;NTE4MzY3QDMxMzkyZTMzMmUzMGZIYlJCOU5GQm9yODJ4ZnVUbGtmQUNRT2lRbXAvN01CMlVRUXljUTAzMnM9;NTE4MzY4QDMxMzkyZTMzMmUzMEk4aEJaYVA4N3owN2d6dFdjNHNhaElXV1NwQUF5d1NOaXU1bTRsWHlER2s9;NTE4MzY5QDMxMzkyZTMzMmUzMEpKS0JSTkQ2Tm5QU3g0QVFJSWNUL3RraS9hMi9JV00wTE1qaXR4YW9kZ009;NTE4MzcwQDMxMzkyZTMzMmUzMEIzQkMxeEpqcEMxTDlaZ3o3TFE5dnpES3hhajlRMzlocG0vSmhNLzQwZEU9;NTE4MzcxQDMxMzkyZTMzMmUzMFZMOTcvYkt2bEE4NHpBZjZycGIzU1puaXpNSllMOGRDaVhDajlxbW0wTnc9;NTE4MzcyQDMxMzkyZTMzMmUzMFk0d0VTNWsvb09wb08vRXVGTEF1VzlJY2VVK0VRcHY4ZXlGZzA4Y0hFdDQ9;NTE4MzczQDMxMzkyZTMzMmUzME1NYTdNdnN0UWxLdHVvZ2JxZzhyRHhBMTZvcFJvTXFDY3ltYzQzSHd3WEk9 ";
SyncfusionLicenseProvider.RegisterLicense(licenseKey);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


//builder.Services.AddSingleton(services =>
//{
//    // Get the service address from appsettings.json
//    var config = services.GetRequiredService<IConfiguration>();
//    var backendUrl = config["BackendUrl"];

//    // If no address is set then fallback to the current webpage URL
//    if (string.IsNullOrEmpty(backendUrl))
//    {
//        var navigationManager = services.GetRequiredService<NavigationManager>();
//        backendUrl = navigationManager.BaseUri;
//    }

//    // Create a channel with a GrpcWebHandler that is addressed to the backend server.
//    //
//    // GrpcWebText is used because server streaming requires it. If server streaming is not used in your app
//    // then GrpcWeb is recommended because it produces smaller messages.
//    var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());

//    return GrpcChannel.ForAddress(backendUrl, new GrpcChannelOptions { HttpHandler = httpHandler });
//});


builder.Services.AddHttpClient("Notes2022.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Notes2022.ServerAPI"));

builder.Services.AddBlazoredModal();

builder.Services.AddApiAuthorization();

builder.Services.AddSyncfusionBlazor();

builder.Services.AddBlazoredSessionStorage();

if (Globals.UserDataList is null)
{
    Globals.UserDataList = new List<UserData>();
    Globals.StartupDateTime = DateTime.Now.ToUniversalTime();
}


await builder.Build().RunAsync();
