using System;
using CORMobileApp.Helpers;
using System.IO;

namespace CORMobileApp.iOS.Helpers
{
    class IOSFileSystem : IFileSyetem
    {
        public string GetWorkingDir()
        {
            string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"..", "Library");
            return filename;
        }

        public string readfakeinstrumentData() {
            return "{}";
        }
    }
}
