using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ZFrameWork
{
    public static class GameSettings
    {
        private static PlatformDefines _platform;
        public static PlatformDefines Platform { get { return _platform; } }
        public static GameSettingConfig gameSettingConfig { get;private set;}
        public static void Init(System.Action onFinish) {
            InitPlatform();
            LoadConfigByString<GameSettingConfig>(GameDefine.SettingFile, (t) => {
                gameSettingConfig = t;
                onFinish?.Invoke();
            });
        }
        private static void InitPlatform() {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _platform = PlatformDefines.Android;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _platform = PlatformDefines.IOS;
                    break;
                case RuntimePlatform.WindowsPlayer:
                    _platform = PlatformDefines.PC;
                    break;
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                    _platform = PlatformDefines.Editor;
                    break;
            }
            FileUtility.SetPlatform(_platform);
        }
        public static void LoadConfigByString<T>(string fileName,System.Action<T> onFinish) where T : new()
        {
            var relatePath = GameDefine.SettingFolder + "/" + fileName;
            Debug.Log("relatePath is :" + relatePath);
            var bytes = FileUtility.FilePlatform.LoadBytesAtStreamingAsstetsFolder(relatePath);
            var text = System.Text.UTF8Encoding.UTF8.GetString(bytes);
            //var text = BetterStreamingAssets.ReadAllText(relatePath);
            if (text == null)
            {
                onFinish?.Invoke(default(T));
            }
            else
            {
                onFinish?.Invoke(text.FromJson<T>());
            }
        }
    }
    
    public class GameSettingConfig
    {
        [SerializeField]
        public string Version = "0.0.0";
        [SerializeField]
        public string ResServerUrl = "https://192.168.1.27:8080";
        [SerializeField]
        public bool UseResServer = true;
        [SerializeField]
        public bool IsDebug = true;
    }
}

