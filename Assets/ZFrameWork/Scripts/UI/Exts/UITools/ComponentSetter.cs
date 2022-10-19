using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ComponentPather
{
    private string Path;
    public ComponentType Type;
    public GameObject GO;

    private GameObject parentTransform;
    private Component component;

    public string GetCsFileString(int pIndex, GameObject Parent, int pTitleIndex, bool pIsOri, string pForceTitleName)
    {
#if UNITY_EDITOR
        var textData = ComponentDef.ComponentBinder[Type];
        var value = textData.GetCsValue(GO.name);
        string defaultName = "fullData";
        string titleName = "fullData";
        var path = SetComponenPathByGO(Parent);
        if (Type == ComponentType.ComponentSetter)
        {
            titleName += pTitleIndex;
            string con = "\t\tvar " + titleName + " = new ComponentSetterFullData();\n";
            var componentSetter = GO.GetComponent<ComponentSetter>();
            var con1 = Game.Editor.ComponentToLua.GetWidgetCS(componentSetter, pTitleIndex, false, null);
            string con2 = "{0}(\"{1}\",{2})";
            string result1 = string.Format(con2, textData.CS, path, titleName);
            string con3 = "\t\t{0}.AddComponentData({1},{2});";
            string result2 = string.Format(con3, defaultName, pIndex, result1);
            return con + con1 + result2;
        }
        else if (Type == ComponentType.TypeMaker)
        {
            if (pTitleIndex > 0 && !pIsOri)
            {
                titleName += pTitleIndex;
            }
            var con = "\t\tvar count = 1;\n";
            var con1 = "\t\t{0}.AddComponentData({1},{2}";
            var con2 = "{0}(\"{1}\",{2},(oGO,oIndex)";
            var con3 = "\t\t\t///ComponentSetter\n";
            var con4 = "\t\t\tvar comSetter = oGO.GetComponent<ComponentSetter>();\n";
            var con5 = "\t\t\t///ComponentSetterFullData\n";
            var con6 = "\t\t\tvar fullDataTemp = new ComponentSetterFullData();\n";
            var componentSetter = GO.GetComponentInChildren<ComponentSetter>();
            var wightString = Game.Editor.ComponentToLua.GetWidgetCS(componentSetter, pTitleIndex, false, "\tfullDataTemp");
            var con7 = "\t\t\tcomSetter.SetComponentDataByFullData(fullDataTemp);\n";
            var con8 = "\t\t}));";
            var result1 = string.Format(con2, textData.CS, path, "count")+ "=>{\n";
            var result2 = string.Format(con1, titleName, pIndex, result1);
            var result = result2;
            return con + result + con3 + con4 + con5 + con6 + wightString + con7 + con8;
        }
        else
        {
            if (pTitleIndex > 0 && !pIsOri)
            {
                titleName += pTitleIndex;
            }
            string con1 = "\t\t{0}.AddComponentData({1},{2});";
            string l_str = "";
            if (!string.IsNullOrEmpty(pForceTitleName))
            {
                titleName = pForceTitleName;
                l_str = "";
            }
            string con2 = "{0}(\"{1}\",{2})";
            if (Type == ComponentType.Text || Type == ComponentType.TextMesh)
            {
                var text = GO.GetComponent<Text>();
                {
                    string con3 = "\"{0}\"";
                    value = string.Format(con3, text.text);
                }
            }
            string result1 = string.Format(con2, textData.CS, path, value);
            string result2 = string.Format(con1, titleName, pIndex, result1);
            string result = l_str + result2;
            return result;
        }
#endif
        return "";
    }

    public string GetLuaFileString(int pIndex,GameObject Parent,int pTitleIndex,bool pIsOri,string pForceTitleName)
    {
#if UNITY_EDITOR
        var textData = ComponentDef.ComponentBinder[Type];
        var value = textData.GetValue(GO.name);
        string defaultName = "fullData";
        string titleName = "fullData";
        var path = SetComponenPathByGO(Parent);
        if (Type == ComponentType.ComponentSetter)
        {
            titleName += pTitleIndex;
            string l_str = "\t---@type CS.ComponentSetterFullData\n";
            string con = "\tlocal " + titleName + " = CS.ComponentSetterFullData()\n";
            var componentSetter = GO.GetComponent<ComponentSetter>();
            var con1 = Game.Editor.ComponentToLua.GetWidget(componentSetter, pTitleIndex, false,null);
            string con2 = "{0}(\"{1}\",{2})";
            string result1 = string.Format(con2, textData.Lua, path, titleName);
            string con3 = "\t{0}:AddComponentData({1},{2})";
            string result2 = string.Format(con3, defaultName, pIndex, result1);
            return l_str + con + con1 + result2;
        }
        else if (Type == ComponentType.TypeMaker) 
        {
            if (pTitleIndex > 0 && !pIsOri)
            {
                titleName += pTitleIndex;
            }
            var con = "\tlocal count = 1\n";
            var con1 = "\t{0}:AddComponentData({1},{2}";
            var con2 = "{0}(\"{1}\",{2},function(oGO,oIndex)\n";
            var con3 = "\t\t---@type CS.ComponentSetter\n";
            var con4 = "\t\tlocal comSetter = oGO:GetComponent(\"ComponentSetter\")\n";
            var con5 = "\t\t---@type CS.ComponentSetterFullData\n";
            var con6 = "\t\tlocal fullDataTemp = CS.ComponentSetterFullData()\n";
            var componentSetter = GO.GetComponentInChildren<ComponentSetter>();
            var wightString = Game.Editor.ComponentToLua.GetWidget(componentSetter, pTitleIndex, false, "\tfullDataTemp");
            var con7 = "\t\tcomSetter:SetComponentDataByFullData(fullDataTemp)\n";
            var con8 = "\tend))";
            var result1 = string.Format(con2, textData.Lua, path, "count");
            var result2 = string.Format(con1, titleName, pIndex, result1);
            var result =  result2;
            return con +　result + con3+  con4 + con5+con6+ wightString + con7 + con8;
        }
        else
        {
            if (pTitleIndex > 0 && !pIsOri)
            {
                titleName += pTitleIndex;
            }
          
            string con1 = "\t{0}:AddComponentData({1},{2})";
            string l_str = "\t---@type " + textData.Lua + "\n";
            if (!string.IsNullOrEmpty(pForceTitleName))
            {
                titleName = pForceTitleName;
                l_str = "\t\t---@type " + textData.Lua + "\n";
            }
            string con2 = "{0}(\"{1}\",{2})";
            if (Type == ComponentType.Text || Type == ComponentType.TextMesh)
            {
                var text = GO.GetComponent<Text>();
                {
                    string con3 = "\"{0}\"";
                    value = string.Format(con3, text.text);
                }
            }
            string result1 = string.Format(con2, textData.Lua, path, value);
            string result2 = string.Format(con1, titleName, pIndex, result1);
            string result = l_str + result2;
            return result;
        }
#endif
        return "";
    }
    public string SetComponenPathByGO(GameObject pParent)
    {
        if (pParent != null && GO != null)
        {
            parentTransform = pParent;
            if (GO.transform == pParent.transform)
            {
                Path = "";
            }
            else
            {
                ComponentDef.FindRelatedPath(GO.transform, pParent.transform, out Path);
            }
           
        }
        return Path;
    }

    public Transform GetTransformByPath()
    {
        if (GO != null)
            return GO.transform;
        if (parentTransform != null)
            return parentTransform.transform.Find(Path);
        return null;
    }
    public Component GetComponent()
    {
        if (component != null){
            return component;
        }
        var transform = GetTransformByPath();
        if (transform)
        {
            var stringType = ComponentDef.ComponentBinder[Type];
            component = transform.gameObject.GetComponent(stringType.Com);
            return component;
        }
        return null;
    }
    public void SetComponentData(ComponentData pComponentData)
    {
        var com = GetComponent();
        pComponentData.OnLoadComponent(com, pComponentData.ComInfo);
    }
}


public class ComponentSetter : MonoBehaviour
{
    public List<ComponentPather> ComponentPathers = new List<ComponentPather>();

    private Dictionary<string, List<ComponentPather>> ComponentMap = new Dictionary<string, List<ComponentPather>>();
    private void Awake()
    {
        OnIntOnce();
    }
    private bool isInitOnce = false;
    private void OnIntOnce() {
        if (isInitOnce) return;isInitOnce = true;
        ComponentMap.Clear();
        foreach (var temp in ComponentPathers)
        {
            var path = temp.SetComponenPathByGO(this.gameObject);
            if (path != null)
            {
                if (ComponentMap.ContainsKey(path))
                {
                    ComponentMap[path].Add(temp);
                }
                else
                {
                    ComponentMap.Add(path, new List<ComponentPather>() { temp });
                }
            }
        }
    }
    public Component GetComponent(string pPath,string pStringKey) {
        var transform = this.gameObject.transform.Find(pPath);
        if (transform != null)
            return transform.gameObject.GetComponent(pStringKey);
        return null;
    }
 
    public void SetComponentDataByFullData(ComponentSetterFullData pComSetFullData) {
       
        var list = pComSetFullData.GetComponetnDatas();
        SetComponentDatas(list);
        //var dcCount = DCGeter.Instance.GetDcCount(this.gameObject) ;
        //Debug.LogError($"DC Count is {dcCount}");

    }
    public void SetComponentDatas(List<ComponentData> pComponentDatas)
    {
        foreach (var temp in pComponentDatas)
        {
            SetOneComponentData(temp);
        }
    }
    public void SetOneComponentData(ComponentData pComponentData)
    {
        var pather = GetComponentPather(pComponentData.Path, pComponentData.ComType);
        if(pather != null)
        {
            pather.SetComponentData(pComponentData);
        }
    }
    private ComponentPather GetComponentPather(string pPath,ComponentType pType)
    {
        if (ComponentMap.Count < 1) {
            OnIntOnce();
        }
        if (ComponentMap.ContainsKey(pPath))
        {
            foreach (var temp in ComponentMap[pPath])
            {
                if (temp.Type == pType)
                    return temp;
            }
        }
        return null;
    }
#if UNITY_EDITOR
    private Transform[] AddTargetComponents(GameObject pTarget) {
        if (!ignoreList.Contains(pTarget))
        {
            var nameT = pTarget.name;
            if (nameT.StartsWith("_"))
            {
                var comSetter = pTarget.GetComponent<ComponentSetter>();
                if (comSetter != null)
                {
                    var childs = comSetter.gameObject.GetComponentsInChildren<Transform>(true);
                    comSetter.PickAllComponent();
                    AddComponent(pTarget, ComponentType.ComponentSetter);
                    return childs;
                }
                else 
                {
                    var maker = pTarget.GetComponent<TypeMaker>();
                    if (maker != null)
                    {
                        var childs = pTarget.GetComponentsInChildren<Transform>(true);
                        comSetter = pTarget.GetComponentInChildren<ComponentSetter>();
                        if(comSetter!= null)
                            comSetter.PickAllComponent();
                        AddComponent(pTarget, ComponentType.TypeMaker);
                        return childs;
                    }
                    else
                    {
                        foreach (var temp in ComponentDef.ComponentSpecLabel)
                        {
                            if (nameT.Contains(temp.Key))
                            {
                                AddComponent(pTarget, temp.Value);
                            }
                        }
                    }
                }
            }
        }
        return null;
    }
#endif
    private void AddComponent(GameObject pTarget,ComponentType pType) {
        var componentPather = new ComponentPather();
        componentPather.GO = pTarget;
        componentPather.Type = pType;
        ComponentPathers.Add(componentPather);
    }

    private List<GameObject> ignoreList = new List<GameObject>();
#if UNITY_EDITOR
    [Button("PickAllComponent")]
    public void PickAllComponent() {
        ComponentPathers.Clear();
        ignoreList.Clear();
        ignoreList.Add(this.gameObject);
        var nameT = this.gameObject.name;
        foreach (var temp in ComponentDef.ComponentSpecLabel)
        {
            if (nameT.Contains(temp.Key))
            {
                AddComponent(this.gameObject, temp.Value);
            }
        }
        var childs = this.gameObject.GetComponentsInChildren<Transform>(true);
        foreach (var temp in childs) {
           var childs_ =  AddTargetComponents(temp.gameObject);
            if (childs_ != null)
            {
                foreach (var temp_ in childs_)
                {
                    ignoreList.Add(temp_.gameObject);
                }
            }
        }
    }
    private bool CheckValid(){
        var regex = new Regex(@"^[a-zA-Z0-9_]+$");
        if (!regex.IsMatch(gameObject.name)){
            Debug.LogError("创建脚本失败 名字中包含非法字符 (请检查预制体名字之后是否多出一个空格):" + gameObject.name);
            return false;
        }
        return true;
    }
    [Button("创建UIBase.cs文件")]
    private void CreateUIBaseFile()
    {
        if (!CheckValid())
            return;
        var path = UnityEditor.EditorUtility.OpenFolderPanel("Select ", "ZFrameWork/Scripts/UI/Exts/Panels/", "*.*");
        if (!string.IsNullOrEmpty(path))
        {
            path = path.Substring(path.IndexOf("UI/"));
            var index = path.LastIndexOf(".");
            if (index > 0)
                path = path.Substring(0, index);
            string l_filePath = path + "/" + gameObject.name;
            Game.Editor.ComponentToLua.GenerateUIBaseCSFile(l_filePath, this);
        }
    }
    [Button("创建UIBaseCtrl.cs文件")]
    private void CreateUIBaseCtrlFile()
    {
        if (!CheckValid())
            return;
        var path = UnityEditor.EditorUtility.OpenFolderPanel("Select ", "ZFrameWork/Scripts/UI/Exts/Panels/", "*.*");
        if (!string.IsNullOrEmpty(path))
        {
            path = path.Substring(path.IndexOf("UI/"));
            var index = path.LastIndexOf(".");
            if (index > 0)
                path = path.Substring(0, index);
            string l_filePath = path + "/" + gameObject.name;
            Game.Editor.ComponentToLua.GenerateUIBaseCtrlCSFile(l_filePath, this);
        }
    }
    [Button("创建UIViewLua .lua文件")]
    private void CreateUIViewLuaFile()
    {
        if (!CheckValid())
            return;
        var path = UnityEditor.EditorUtility.OpenFolderPanel("Select ", GameDefine.LuaResources + "UI/","*.*");
        if (!string.IsNullOrEmpty(path))
        {
            path = path.Substring(path.IndexOf("UI/"));
            var index = path.LastIndexOf(".");
            if (index > 0)
                path = path.Substring(0, index);
            string l_filePath = path + "/" + gameObject.name;
            Game.Editor.ComponentToLua.GenerateUILuaFile(l_filePath, this);
        }
    }

    [Button("创建UICtrlLua .lua文件")]
    private void CreateUICtrlLuaFile()
    {
        if (!CheckValid())
            return;
        var path = UnityEditor.EditorUtility.OpenFolderPanel("Select UICtrl File", GameDefine.LuaResources + "UI/", "*.*");
        if (!string.IsNullOrEmpty(path))
        {
            path = path.Substring(path.IndexOf("UI/"));
            var index = path.LastIndexOf(".");
            if (index > 0)
                path = path.Substring(0, index);
            string l_filePath = path + "/" + gameObject.name +"Ctrl";
            Game.Editor.ComponentToLua.GenerateUICtrlLuaFile(l_filePath, this);
        }
    }
#endif
}
