#if UNITY_EDITOR
namespace Game.Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using UnityEditor;
    using UnityEngine;
    public class ComponentToLua
    {
        public static void GenerateUICtrlLuaFile(string pFilePath, ComponentSetter pComponent)
        {
            var modeulFilepath = Application.dataPath + "/" + GameDefine.LuaResources + "/UI/Exts/Test/XPanelCtrl.lua";
            pFilePath = pFilePath.Replace("\\", "/");
            var filepath = Application.dataPath + "/" + GameDefine.LuaResources + "/" + pFilePath + ".lua";
            var fileName = filepath.Substring(filepath.LastIndexOf("/") + 1);
            fileName = fileName.Replace("Ctrl.lua", "");
            var releatePath = pFilePath.Replace("UI/Exts/", "");
            releatePath = releatePath.Replace("Ctrl", "");
            if (File.Exists(filepath))
            {
                if (UnityEditor.EditorUtility.DisplayDialog("面板的LuaCtrl文件已经存在", "是否删除重新生成 ?", "确定", "取消"))
                {
                    File.Delete(filepath);
                    StartGeneralCtrl(pComponent, modeulFilepath, fileName, filepath, releatePath);
                }
            }
            else
            {
                StartGeneralCtrl(pComponent, modeulFilepath, fileName, filepath, releatePath);
            }
        }
        private static void StartGeneralCtrl(ComponentSetter pComponent, string modeulFilepath, string fileName, string filepath, string pReleatePath)
        {
            var modeulContext = ReadModel(modeulFilepath);
            Debug.Log(fileName);
            //var assetPath = AssetDatabase.GetAssetPath(pComponent.gameObject);
            var assetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(pComponent.gameObject);
            modeulContext = modeulContext.Replace("xx.prefab", assetPath);
            modeulContext = modeulContext.Replace("Test/XPanel", pReleatePath);
            modeulContext = modeulContext.Replace("XPanel", fileName);
            var widgetContext = GetWidget(pComponent, 0, true, null);
            modeulContext = modeulContext.Replace("--SetComponentDatas", widgetContext);
            WriteFile(filepath, modeulContext);
        }
        public static string GetWidget(ComponentSetter pComponent, int pTitleIndex, bool pIsOri, string pForceTitleName)
        {
            string context = "";
            int index = 0;
            foreach (var temp in pComponent.ComponentPathers)
            {
                index++;
                var isAddIndex = temp.Type == ComponentType.ComponentSetter && pIsOri;
                if (isAddIndex)
                    pTitleIndex++;
                var tempContext = temp.GetLuaFileString(index, pComponent.gameObject, pTitleIndex, pIsOri, pForceTitleName);

                context += tempContext + "\n";
            }
            return context;
        }
        public static string GetWidgetCS(ComponentSetter pComponent, int pTitleIndex, bool pIsOri, string pForceTitleName)
        {
            string context = "";
            int index = 0;
            foreach (var temp in pComponent.ComponentPathers)
            {
                index++;
                var isAddIndex = temp.Type == ComponentType.ComponentSetter && pIsOri;
                if (isAddIndex)
                    pTitleIndex++;
                var tempContext = temp.GetCsFileString(index, pComponent.gameObject, pTitleIndex, pIsOri, pForceTitleName);

                context += tempContext + "\n";
            }
            return context;
        }
        public static void GenerateUILuaFile(string pFilePath, ComponentSetter pComponent)
        {
            var modeulFilepath = Application.dataPath + "/" + GameDefine.LuaResources + "/UI/Exts/Test/XPanel.lua";
            pFilePath = pFilePath.Replace("\\", "/");
            var filepath = Application.dataPath + "/" + GameDefine.LuaResources + "/" + pFilePath + ".lua";
            var fileName = filepath.Substring(filepath.LastIndexOf("/") + 1);
            fileName = fileName.Replace(".lua", "");
            if (File.Exists(filepath))
            {
                if (UnityEditor.EditorUtility.DisplayDialog("面板的Lua文件已经存在", "是否删除重新生成 ?", "确定", "取消"))
                {
                    File.Delete(filepath);
                    StartGeneral(pComponent, modeulFilepath, fileName, filepath);
                }
            }
            else
            {
                StartGeneral(pComponent, modeulFilepath, fileName, filepath);
            }
        }
        private static void StartGeneral(ComponentSetter pComponent, string modeulFilepath, string fileName, string filepath)
        {
            var modeulContext = ReadModel(modeulFilepath);
            Debug.Log(fileName);
            modeulContext = modeulContext.Replace("XPanel", fileName);
            WriteFile(filepath, modeulContext);
        }
      
        public static void GenerateUIBaseCSFile(string pFilePath, ComponentSetter pComponent)
        {
            var rootPath = Application.dataPath + "/ZFrameWork/Scripts/UI/Model/";
            var modeulFilepath = $"{rootPath}XPanel.cs";// Application.dataPath + "/ZFrameWork/Scripts/UI/Model/XPanel.cs";
            pFilePath = pFilePath.Replace("\\", "/");
            var filepath = Application.dataPath + "/ZFrameWork/Scripts/" + pFilePath + ".cs";
            var fileName = filepath.Substring(filepath.LastIndexOf("/") + 1);
            fileName = fileName.Replace(".cs", "");
            if (File.Exists(filepath))
            {
                if (UnityEditor.EditorUtility.DisplayDialog("面板的cs文件已经存在", "是否删除重新生成 ?", "确定", "取消"))
                {
                    File.Delete(filepath);
                    StartGeneral(pComponent, modeulFilepath, fileName, filepath);
                }
            }
            else
            {
                StartGeneral(pComponent, modeulFilepath, fileName, filepath);
            }
        }
        public static void GenerateUIBaseCtrlCSFile(string pFilePath, ComponentSetter pComponent)
        {
            var modeulFilepath = Application.dataPath + "/ZFrameWork/Scripts/UI/Model/XPanelCtrl.cs";
            pFilePath = pFilePath.Replace("\\", "/");
            var filepath = Application.dataPath + "/ZFrameWork/Scripts/" + pFilePath + "Ctrl.cs";
            var fileName = filepath.Substring(filepath.LastIndexOf("/") + 1);
            fileName = fileName.Replace("Ctrl.cs", "");
            var releatePath = pFilePath.Replace("UI/Exts/", "");
            releatePath = releatePath.Replace("Ctrl", "");
            if (File.Exists(filepath))
            {
                if (UnityEditor.EditorUtility.DisplayDialog("面板的LuaCtrl文件已经存在", "是否删除重新生成 ?", "确定", "取消"))
                {
                    File.Delete(filepath);
                    StartGeneralCtrlCS(pComponent, modeulFilepath, fileName, filepath, releatePath);
                }
            }
            else
            {
                StartGeneralCtrlCS(pComponent, modeulFilepath, fileName, filepath, releatePath);
            }
        }
        private static void StartGeneralCtrlCS(ComponentSetter pComponent, string modeulFilepath, string fileName, string filepath, string pReleatePath)
        {
            var modeulContext = ReadModel(modeulFilepath);
            Debug.Log(fileName);
            //var assetPath = AssetDatabase.GetAssetPath(pComponent.gameObject);
            var assetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(pComponent.gameObject);
            modeulContext = modeulContext.Replace("xx.prefab", assetPath);
            //modeulContext = modeulContext.Replace("Test/XPanel", pReleatePath);
            modeulContext = modeulContext.Replace("XPanel", fileName);
            var widgetContext = GetWidgetCS(pComponent, 0, true, null);
            modeulContext = modeulContext.Replace("//SetComponentDatas", widgetContext);
            WriteFile(filepath, modeulContext);
        }

        public static string ReadModel(string pFilePath)
        {
            StreamReader l_streamReader = new StreamReader(pFilePath, Encoding.UTF8);
            string l_context = l_streamReader.ReadToEnd();
            l_streamReader.Close();
            return l_context;
        }
        public static void WriteFile(string pFilePath, string pContext)
        {
            FileStream l_fs = new FileStream(pFilePath, FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter l_streamWriter = new StreamWriter(l_fs);
            l_streamWriter.WriteLine(pContext);
            l_streamWriter.Close();
            l_fs.Close();
            AssetDatabase.Refresh();
            Debug.Log(string.Format("File has created at path :------{0}.", pFilePath));
        }
    }
}
#endif





