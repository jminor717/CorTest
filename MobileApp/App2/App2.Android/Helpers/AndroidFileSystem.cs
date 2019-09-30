using Android.App;
using Android.Content.Res;
using Android.OS;
using App2.Helpers;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace App2.Droid
{
    class AndroidFileSystem : IFileSyetem
    {
        public string GetWorkingDir()
        {
            string fullPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            return fullPath;
        }
        public string readfakeinstrumentData()
        {
            AssetManager Assets = Forms.Context.Assets;
            string text = "";
            using (var reader = new StreamReader(Assets.Open("offlineData.json")))
            {
                text = reader.ReadToEnd();
            }
            //System.Diagnostics.Debug.WriteLine(text.Length);
            return text;
        }
    }
}