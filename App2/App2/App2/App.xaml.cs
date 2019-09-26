using App2.Database;
using App2.Helpers;
using App2.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace App2
{
    public partial class App : Application
    {
        static DatabaseOperations database;

        public static IFileSyetem fs { get; private set; }
        public static INotificationControl msger { get; private set; }
        public static IHTTPs Https { get; private set; }
        public static string backend { get; private set; }
        public static void InitFS(IFileSyetem userPreferencesImpl)
        {
            App.fs = userPreferencesImpl;
        }
        public static void Initmsger(INotificationControl userPreferencesImpl) {
            App.msger = userPreferencesImpl;
        }
        public static void InitHttps(IHTTPs hhttpp) {
            App.Https = hhttpp;
        }
        public App()
        {
            //https://10.16.176.111:5001
            //https://10.0.2.2:5001
            backend = "https://10.16.176.111:5001";
            InitializeComponent();

            SetMainPage();
        }

        public static DatabaseOperations Database
        {
            get
            {
                if (database == null)
                {
                    string path = Path.Combine(App.fs.GetWorkingDir(), "TodoSQLite.db3");
                    database = new DatabaseOperations(path);
                }
                return database;
            }
        }


        public static string getFakeData()
        {
            /*
            var assembly = typeof(App).GetTypeInfo().Assembly;//IntrospectionExtensions
            var stuf = assembly.GetManifestResourceNames();
            Debug.WriteLine(stuf);

            Stream stream = assembly.GetManifestResourceStream("App2.offlineData.json");
            string text = "";
            using (var reader = new System.IO.StreamReader(stream)) {
                text = reader.ReadToEnd();
            }
            return text;
            //*/
            return App.fs.readfakeinstrumentData();
        }

        public static void SetMainPage()
        {
            Current.MainPage = new NavigationPage(new InstrumentListPage())
            {
                Title = "Browse",
                Icon = Device.OnPlatform("tab_feed.png", null, null)
            };
            /*  new TabbedPage
          {
              Children =
              {
                  new NavigationPage(new ItemsPage())
                  {
                      Title = "Browse",
                      Icon = Device.OnPlatform("tab_feed.png",null,null)
                  },
                  /*
                  new NavigationPage(new AboutPage())
                  {
                      Title = "About",
                      Icon = Device.OnPlatform("tab_about.png",null,null)
                  },
              }
          };*/
        }
    }
}
