using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace ZFrameWork {

    public class GameLogic
    {
        private static GameLogic instance;
        public static GameLogic Instance { get { 
                if(instance == null)
                    instance = new GameLogic();
                return instance; 
        } }
        private System.Reflection.Assembly gameAss;

        public void LoadGameDll()
        {
            byte[] commonBytes = FileUtility.FilePlatform.LoadBytesSafeAtPersistencePath("common");
            AssetBundle commonAb = AssetBundle.LoadFromMemory(commonBytes);
#if !UNITY_EDITOR
            TextAsset dllBytes1 = commonAb.LoadAsset<TextAsset>("HotFix.dll.bytes");
            System.Reflection.Assembly.Load(dllBytes1.bytes);
            TextAsset zframeWork = commonAb.LoadAsset<TextAsset>("ZFrameWork.dll.bytes");
            System.Reflection.Assembly.Load(zframeWork.bytes);
            TextAsset dllBytes2 = commonAb.LoadAsset<TextAsset>("HotFix2.dll.bytes");
            gameAss = System.Reflection.Assembly.Load(dllBytes2.bytes);
#else
            gameAss = AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "HotFix2");
#endif
        }
        private void LoadGameDllGame()
        {
            byte[] commonBytes = FileUtility.FilePlatform.LoadBytesSafeAtPersistencePath("common");
            AssetBundle commonAb = AssetBundle.LoadFromMemory(commonBytes);
#if !UNITY_EDITOR
            TextAsset dllBytes1 = commonAb.LoadAsset<TextAsset>("HotFix.dll.bytes");
            System.Reflection.Assembly.Load(dllBytes1.bytes);
            TextAsset zframeWork = commonAb.LoadAsset<TextAsset>("ZFrameWork.dll.bytes");
             System.Reflection.Assembly.Load(zframeWork.bytes);
            TextAsset dllBytes2 = commonAb.LoadAsset<TextAsset>("HotFix2.dll.bytes");
            gameAss = System.Reflection.Assembly.Load(dllBytes2.bytes);
#else
            gameAss = AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "ZFrameWork");
#endif
        }
        public void RunMainGame()
        {
            if (gameAss == null)
            {
                UnityEngine.Debug.LogError("dllŒ¥º”‘ÿ");
                return;
            }
            var appType = gameAss.GetType("GameAppStartUp");
            var mainMethod = appType.GetMethod("StartUp");
            mainMethod.Invoke(null, null);
        }
        public void RunMain()
        {
            if (gameAss == null)
            {
                UnityEngine.Debug.LogError("dllŒ¥º”‘ÿ");
                return;
            }
            var appType = gameAss.GetType("App");
            var mainMethod = appType.GetMethod("Main");
            mainMethod.Invoke(null, null);
        }
    }

}
