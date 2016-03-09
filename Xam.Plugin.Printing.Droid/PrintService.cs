using System;
using Android.Content;
using Android.Print;

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
	}
}

