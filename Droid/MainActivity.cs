using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Plugin.Printing;
using Plugin.Printing.Droid;

namespace Printing.Droid
{
	[Activity (Label = "Printing.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

			Splat.Locator.CurrentMutable.Register (
				() => new PrintService(this),
				typeof(IPrintService)
			);

			LoadApplication (new App ());
		}
	}
}

