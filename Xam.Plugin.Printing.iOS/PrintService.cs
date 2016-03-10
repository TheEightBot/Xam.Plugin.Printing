using System;
using UIKit;
using Foundation;
using System.Threading.Tasks;

namespace Plugin.Printing.iOS
{
	public class PrintService : IPrintService
	{
		public PrintService ()
		{
		}

		public void PrintFile(string jobName, string pdfPath){
			var printInfo = UIPrintInfo.PrintInfo;
			printInfo.Duplex = UIPrintInfoDuplex.LongEdge;
			printInfo.OutputType = UIPrintInfoOutputType.General;
			printInfo.JobName = jobName;

			var printer = UIPrintInteractionController.SharedPrintController;
			printer.PrintInfo = printInfo;

			var data = NSData.FromFile(pdfPath);

			printer.PrintingItem = data;

			printer.ShowsPageRange = true;

			printer.Present(true, (handler, completed, err) => {
				
				data?.Dispose();
				data = null;

				if(!completed && err !=null) {
					System.Diagnostics.Debug.WriteLine("Failed to print");
				}
			});
		}

		public async Task<bool> PrintWeb(string jobName, string url){
			var printInfo = UIPrintInfo.PrintInfo;
			printInfo.Duplex = UIPrintInfoDuplex.LongEdge;
			printInfo.OutputType = UIPrintInfoOutputType.General;
			printInfo.JobName = jobName;

			var printer = UIPrintInteractionController.SharedPrintController;
			printer.PrintInfo = printInfo;

			//var data = NSData.FromUrl(url);

			var webView = new UIWebView ();
			webView.LoadRequest (NSUrlRequest.FromUrl (new NSUrl (url)));

			var tsc = new TaskCompletionSource<bool> ();

			EventHandler loadFinished = (s, e) => {
				tsc.SetResult(true);
			};

			webView.LoadFinished += loadFinished;

			var result = await tsc.Task;

			if (!result)
				return false;

			webView.LoadFinished -= loadFinished;

			printer.PrintFormatter = webView.ViewPrintFormatter;

			printer.ShowsPageRange = true;

			printer.Present(true, (handler, completed, err) => {
				if(!completed && err !=null) {
					System.Diagnostics.Debug.WriteLine("Failed to print");
				}
			});

			return true;
		}
	}
}

