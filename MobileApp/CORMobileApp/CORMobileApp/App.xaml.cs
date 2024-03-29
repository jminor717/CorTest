﻿using CORMobileApp.Database;
using CORMobileApp.Helpers;
using CORMobileApp.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CORMobileApp
{
    public partial class App : Application
    {
        static DatabaseOperations database;

        public static theme theme { get; private set; }
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
            //backend = "https://10.16.176.111:5001";
            backend = "https://10.0.2.2:5001";
            theme = new theme();
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

            Stream stream = assembly.GetManifestResourceStream("CORMobileApp.offlineData.json");
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
            string icon = null;
            switch (Device.RuntimePlatform) {
                case "iOS":
                    icon = "tab_feed.png";
                    break;
            }

            Current.MainPage = new NavigationPage(new InstrumentListPage())
            {
                Title = "Browse",
                IconImageSource = icon

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

    public class theme {
        public Color HomeCard = Color.FromHex("#fff");
        public Color HomebackGround = Color.FromHex("#f4f4f4");
        public Color primary = Color.FromHex("#056b99");
        public Color secondary = Color.FromHex("#067fc3");
        public Color terteiary = Color.FromHex("#4c637b");
        public Color quatrenary = Color.FromHex("#2f5c76");
        public Color alert = Color.FromHex("#d10018");
        public Color warning = Color.FromHex("#ffb032");
        public Color good = Color.FromHex("#008600");

    }
}

