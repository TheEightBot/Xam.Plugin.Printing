using System;
using UIKit;
using Foundation;

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
	}
}

