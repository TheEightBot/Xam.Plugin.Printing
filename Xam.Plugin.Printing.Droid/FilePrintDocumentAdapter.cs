using System;
using Android.Print;
using Java.IO;

namespace Plugin.Printing.Droid
{
	public class FilePrintDocumentAdapter : PrintDocumentAdapter
	{
		string _fileName, _filePath;

		public FilePrintDocumentAdapter (string fileName, string filePath)
		{
			_fileName = fileName;

			_filePath = filePath;
		}

		#region implemented abstract members of PrintDocumentAdapter

		public override void OnLayout (PrintAttributes oldAttributes, PrintAttributes newAttributes, Android.OS.CancellationSignal cancellationSignal, LayoutResultCallback callback, Android.OS.Bundle extras)
		{
			if (cancellationSignal.IsCanceled) {
				callback.OnLayoutCancelled();
				return;
			}

			var pdi = 
				new PrintDocumentInfo
					.Builder(_fileName)
					.SetContentType(Android.Print.PrintContentType.Document)
					.Build();

			callback.OnLayoutFinished(pdi, true);
		}

		public override void OnWrite (PageRange[] pages, Android.OS.ParcelFileDescriptor destination, Android.OS.CancellationSignal cancellationSignal, WriteResultCallback callback)
		{
			InputStream input = null;
			OutputStream output = null;

			try {

				input = new FileInputStream(_filePath);
				output = new FileOutputStream(destination.FileDescriptor);

				byte[] buf = new byte[1024];
				int bytesRead;

				while ((bytesRead = input.Read(buf)) > 0) {
					output.Write(buf, 0, bytesRead);
				}

				callback.OnWriteFinished(new PageRange[]{PageRange.AllPages});

			} catch (FileNotFoundException ee){
				//Catch exception
			} catch (Exception e) {
				//Catch exception
			} finally {
				try {
					input.Close();
					output.Close();
				} catch (IOException e) {
					e.PrintStackTrace();
				}
			}
		}

		#endregion
	}
}

