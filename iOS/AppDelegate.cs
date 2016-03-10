using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Plugin.Printing;
using Plugin.Printing.iOS;

namespace Printing.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

			Splat.Locator.CurrentMutable.Register (
				() => new PrintService(),
				typeof(IPrintService)
			);

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

