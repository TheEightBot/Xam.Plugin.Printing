using System;

using Xamarin.Forms;
using Plugin.Printing;
using Splat;
using System.Net.Http;

namespace Printing
{
	public class App : Application
	{
		Button _printWeb, _printFile;

		ActivityIndicator _loading;

		public App ()
		{
			var printing = Splat.Locator.CurrentMutable.GetService<IPrintService> ();

			var appPath = PCLStorage.FileSystem.Current;

			// The root page of your application
			MainPage = new ContentPage {
				Content = new StackLayout {
					VerticalOptions = LayoutOptions.Center,
					Children = {
						new Label {
							XAlign = TextAlignment.Center,
							Text = "Printing Example"
						},
						(_printWeb = new Button {
							Text = "Print Web",
							Command = new Command(async _ => {
								_printWeb.IsEnabled = false;
								_loading.IsRunning = true;

								try {
									await printing.PrintWeb("print_test", "http://eightbot.com");
								} finally {
									_printWeb.IsEnabled = true;
									_loading.IsRunning = false;
								}
							})
						}),
						(_printFile = new Button {
							Text = "Print PDF",
							Command = new Command(async _ => {
								_printFile.IsEnabled = false;
								_loading.IsRunning = true;

								try {
									var httpClient = new HttpClient();
									var pdfDocument = await httpClient.GetByteArrayAsync("https://github.com/mozilla/pdf.js/raw/master/web/compressed.tracemonkey-pldi-09.pdf");

									var pdfFile = await appPath.LocalStorage.CreateFileAsync("samplePdf.pdf", PCLStorage.CreationCollisionOption.ReplaceExisting);

									using(var openFile = await pdfFile.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
										await openFile.WriteAsync(pdfDocument, 0, pdfDocument.Length);
									
									printing.PrintFile("print_test", pdfFile.Path);
								} finally {
									_printFile.IsEnabled = true;
									_loading.IsRunning = false;
								}
							})
						}),
						(_loading = new ActivityIndicator {})
					}
				}
			};
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

