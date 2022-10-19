using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFrameWork
{

    public class FileUtility
    {
        private static FilePlatform _filePlatform;
        public static FilePlatform FilePlatform {get {return _filePlatform;}}

        public static void SetPlatform(PlatformDefines platformDefines)
        {
            if (platformDefines == PlatformDefines.Editor)
                _filePlatform = new FilePlatformStandalone();
            else if(platformDefines == PlatformDefines.Android)
                _filePlatform = new FilePlatformAndroid();
        }
    }
}
