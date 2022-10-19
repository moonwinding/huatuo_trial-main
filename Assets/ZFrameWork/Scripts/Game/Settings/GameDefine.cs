using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ZFrameWork
{
    public class GameDefine
    {
        public const string SettingFolder = "GameSettings";
        public const string SettingFile = "GameSettings.json";
        public const string TotalVersion = "VersionData/TotalVersion.json";
        public const string TotalVersion_Line = "VersionData/TotalVersionLine.json";
        public const string TotalVersion_Local = "VersionData/TotalVersionLocal.json";
        public const string BuildRoot = "Build";
    }
    public enum PlatformDefines { 
    
        Editor = 0,
        Android = 1,
        IOS = 2,
        PC = 3
    }
}

