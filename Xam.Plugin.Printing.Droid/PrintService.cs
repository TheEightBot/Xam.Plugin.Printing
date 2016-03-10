using System;
using Android.Content;
using Android.Print;
using Android.Webkit;
using System.Threading.Tasks;

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

		public Task<bool> PrintWeb(string jobName, string url){
			var printMgr = _context.GetSystemService(Android.Content.Context.PrintService) as PrintManager;

			if(printMgr == null){
				//do something here
				return Task.FromResult(false);
			}

			var webView = new WebView (_context);

			var tcs = new TaskCompletionSource<bool> ();

			webView.SetWebViewClient (new PrintWebViewClient (_context, tcs));

			webView.LoadUrl (url);

			return tcs.Task;
		}

		class PrintWebViewClient : WebViewClient {

			Context _context;

			TaskCompletionSource<bool> _tcs;

			public PrintWebViewClient (Context context, TaskCompletionSource<bool> tcs)
			{
				_context = context;

				_tcs = tcs;
			}

			public override bool ShouldOverrideUrlLoading (WebView view, string url)
			{
				return false;
			}

			public override void OnPageFinished (WebView view, string url)
			{
				PerformWebPrint (view);

				view.Dispose ();
				view = null;

				_context = null;

				_tcs.TrySetResult (true);
			}

			public override void OnReceivedError (WebView view, ClientError errorCode, string description, string failingUrl)
			{
				_tcs.TrySetResult (false);
			}

			public override void OnReceivedHttpAuthRequest (WebView view, HttpAuthHandler handler, string host, string realm)
			{
				_tcs.TrySetResult (false);
			}

			public override void OnReceivedSslError (WebView view, SslErrorHandler handler, Android.Net.Http.SslError error)
			{
				_tcs.TrySetResult (false);
			}

			private void PerformWebPrint(WebView webView){
				try {
					var printMgr = _context.GetSystemService(Android.Content.Context.PrintService) as PrintManager;

					if(printMgr == null){
						//do something here
						return;
					}

					printMgr.Print(
						webView.Url,
						webView.CreatePrintDocumentAdapter(),
						null);
				} catch (Exception ex) {
					_tcs.TrySetException(ex);
				}

			}
		}
	}
}

