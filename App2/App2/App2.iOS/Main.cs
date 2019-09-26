using UIKit;

namespace App2.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main(string[] args)
		{
            App.InitFS(new Helpers.IOSFileSystem());
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
		}
	}
}
