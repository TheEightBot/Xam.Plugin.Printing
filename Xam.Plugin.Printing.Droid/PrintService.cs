using System;
using Android.Content;
using Android.Print;
using Android.Webkit;

namespace Plugin.Printing.Droid
{
	public class PrintService : IPrintService
	{
		Context _context;

		public PrintService (Context context)
		{
			_context = context;
		}

		public void PrintFile(string jobName, string filePath){
			var printMgr = _context.GetSystemService(Android.Content.Context.PrintService) as PrintManager;

			if(printMgr == null){
				//do something here
				return;
			}

			printMgr.Print(
				jobName,
				new FilePrintDocumentAdapter (jobName, filePath),
				null);
		}

		public void PrintWeb(string jobName, string url){
			var printMgr = _context.GetSystemService(Android.Content.Context.PrintService) as PrintManager;

			if(printMgr == null){
				//do something here
				return;
			}

			var webView = new WebView (_context);
			webView.SetWebViewClient (new PrintWebViewClient ());

			webView.LoadUrl (url);
		}

		class PrintWebViewClient : WebViewClient {
			public override bool ShouldOverrideUrlLoading (WebView view, string url)
			{
				return false;
			}

			public override void OnPageFinished (WebView view, string url)
			{
				PerformWebPrint (view);

				view.Dispose ();
				view = null;
			}

			private void PerformWebPrint(WebView webView){
				var printMgr = _context.GetSystemService(Android.Content.Context.PrintService) as PrintManager;

				if(printMgr == null){
					//do something here
					return;
				}

				printMgr.Print(
					webView.Url,
					webView.CreatePrintDocumentAdapter(),
					null);
			}
		}
	}
}

