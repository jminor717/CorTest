using System;
using App2.Helpers;
using System.IO;

namespace App2.iOS.Helpers
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
