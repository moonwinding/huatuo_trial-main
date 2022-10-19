using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace ZFrameWork
{
    public class GameMain : MonoBehaviour
    {
        // Start is called before the first frame update
        private System.Reflection.Assembly gameAss;
        void Start()
        {
            Debug.Log("GameMain >>11");
            LoadGameDll();
            RunMain();
            //LoadGameDllGame();
            //RunMainGame();
        }


        private void LoadGameDll()
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
                UnityEngine.Debug.LogError("dllδ����");
                return;
            }
            var appType = gameAss.GetType("GameAppStartUp");
            var mainMethod = appType.GetMethod("StartUp");
            mainMethod.Invoke(null,null);
        }
        public void RunMain()
        {
            if (gameAss == null)
            {
                UnityEngine.Debug.LogError("dllδ����");
                return;
            }
            var appType = gameAss.GetType("App");
            var mainMethod = appType.GetMethod("Main");
            mainMethod.Invoke(null, null);
        }
    }
}

