using Microsoft.Maui;
using Microsoft.Maui.Controls;
//using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Application = Microsoft.Maui.Controls.Application;

using System.Diagnostics;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Essentials;
//using Notes2022Preview.View;

using Device = Microsoft.Maui.Controls.Device;


namespace Notes2022Preview
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

			//VersionTracking.Track();

			//MainPage = new NavigationPage(new HomePage());

			MainPage = new MainPage();
        }

		//async void SetUpAppActions()
		//{
		//	try
		//	{
		//		await AppActions.SetAsync(
		//			new AppAction("app_info", "App Info", icon: "app_info_action_icon"),
		//			new AppAction("battery_info", "Battery Info"));

		//		AppActions.OnAppAction += AppActions_OnAppAction;
		//	}
		//	catch (FeatureNotSupportedException ex)
		//	{
		//		Debug.WriteLine($"{nameof(AppActions)} Exception: {ex}");
		//	}
		//}

		//void AppActions_OnAppAction(object sender, AppActionEventArgs e)
		//{
		//	MainPage.Dispatcher.BeginInvokeOnMainThread(async () =>
		//	{
		//		var page = e.AppAction.Id switch
		//		{
		//			"battery_info" => new BatteryPage(),
		//			"app_info" => new AppInfoPage(),
		//			_ => default(Page)
		//		};

		//		if (page is not null)
		//		{
		//			await Application.Current.MainPage.Navigation.PopToRootAsync();
		//			await Application.Current.MainPage.Navigation.PushAsync(page);
		//		}
		//	});
		//}
	}
}
