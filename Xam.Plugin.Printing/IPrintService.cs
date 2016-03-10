using System;
using System.Threading.Tasks;

namespace Plugin.Printing
{
	public interface IPrintService
	{
		void PrintFile(string jobName, string pdfPath);

		Task<bool> PrintWeb(string jobName, string url);
	}
}

