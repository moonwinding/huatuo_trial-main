using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComonentSetInfo {
    public string Com;
    public string Lua;
    public string LuaValue;
    public string CS;
    public string CSValue;
    public Dictionary<string, string> ValueMap = new Dictionary<string, string>() { };
    public Dictionary<string, string> CSMap = new Dictionary<string, string>() { };
    public string GetValue(string pGOName) {
        if (pGOName.Contains("_OpenUI_")) {
            var fileNameReq = "function()\n\t\tlocal ctrl = require(\"UI.Exts.{0}Ctrl\").Create()\n";
            var index = pGOName.LastIndexOf("_");
            var panelName = pGOName.Substring(index+1);
            var ctrlName = string.Format(fileNameReq, panelName);
            var con2 = "\t\tUICtrlManager.BaseOpenCtrl(ctrl,nil)\n\tend";
            return ctrlName + con2;
        }
        foreach (var temp in ValueMap) {
            if (pGOName.Contains(temp.Key))
                return temp.Value;
        }
        return LuaValue;
    }
    public string GetCsValue(string pGOName)
    {
        if (pGOName.Contains("_OpenUI_"))
        {
            //var fileNameReq = "()=>{\n\t\tvar ctrl = new {0}Ctrl\n";
            var index = pGOName.LastIndexOf("_");
            var panelName = pGOName.Substring(index + 1);
            var ctrlName = "()=>{\n\t\tvar ctrl = new " + panelName + "Ctrl();\n";// string.Format(fileNameReq, panelName);
            var con2 = "\t\tUICtrlManager.OpenBaseUI(ctrl,null);\n\t}";
            return ctrlName + con2;
        }
        foreach (var temp in CSMap)
        {
            if (pGOName.Contains(temp.Key))
                return temp.Value;
        }
        return CSValue;
    }
}

public static class ComponentDef
{
    public static Dictionary<ComponentType, ComonentSetInfo> ComponentBinder = new Dictionary<ComponentType, ComonentSetInfo>() {
        {ComponentType.Text,new ComonentSetInfo(){ Com = "Text",Lua = "CS.TextData",LuaValue = "\"--\"",CS = "new TextData",CSValue = "\"--\""} },
        {ComponentType.Image,new ComonentSetInfo(){ Com = "Image",Lua = "CS.ImageData",LuaValue = "nil",CS = "new ImageData",CSValue = "null"} },
        {ComponentType.Button,new ComonentSetInfo()
        {
            Com = "Button",Lua = "CS.ButtonData",LuaValue = "function() end",
            ValueMap = new Dictionary<string, string>()
            {
                { "_Back_","function() UICtrlManager.BaseCloseTop() end" },
                { "_Home_","function() UICtrlManager.BaseRevokeToHome() end" }
            }
            ,CS = "new ButtonData", CSValue = "()=> {}",
            CSMap = new Dictionary<string, string>(){
                { "_Back_","() =>{ UICtrlManager.CloseTopBaseUI(); }" },
                { "_Home_","()  =>{ UICtrlManager.RevokeToHomeBaseUI(); }" }
            }
        }
        },
        {ComponentType.NormalBtn,new ComonentSetInfo(){ Com = "NormalBtn",Lua = "CS.NormalBtnData",LuaValue = "1",CS = "new NormalBtnData",CSValue = "1"} },
        {ComponentType.TextMesh,new ComonentSetInfo(){ Com = "TMPro.TextMeshProUGUI",Lua = "CS.TextMeshProData",LuaValue = "\"--\"",CS = "new TextMeshProData",CSValue = "\"--\""} },
        {ComponentType.Transform,new ComonentSetInfo(){ Com = "Transform",Lua = "CS.TransformData",LuaValue = "Vector3.zero",
            ValueMap = new Dictionary<string, string>(){
                { "_localPos_","Vector3.zero,\"localPos\"" },
                { "_worldPos_","Vector3.zero,\"worldPos\"" },
                { "_localRot_","Vector3.zero,\"localRot\"" },
                { "_worldRot_","Vector3.zero,\"worldRot\"" },
                { "_scale_","Vector3.one,\"scale\"" }
            },
            CS = "new TransformData",CSValue = "Vector3.zero",
            CSMap = new Dictionary<string, string>(){
                { "_localPos_","Vector3.zero,\"localPos\"" },
                { "_worldPos_","Vector3.zero,\"worldPos\"" },
                { "_localRot_","Vector3.zero,\"localRot\"" },
                { "_worldRot_","Vector3.zero,\"worldRot\"" },
                { "_scale_","Vector3.one,\"scale\"" }
            } 
            }
        },
        {ComponentType.ComponentSetter,new ComonentSetInfo(){ Com = "ComponentSetter",Lua = "CS.ComponentSetterData",LuaValue = "\"--\"",CS = "new ComponentSetterData",CSValue = "\"--\""} },
        {ComponentType.TypeMaker,new ComonentSetInfo(){ Com = "TypeMaker",Lua = "CS.TypeMakerData",LuaValue = "\"--\"",CS = "new TypeMakerData",CSValue = "\"--\""} },
    };

    
    public static Dictionary<string, ComponentType> ComponentSpecLabel = new Dictionary<string, ComponentType>() {
        {"_Txt_",ComponentType.Text},
        {"_Img_",ComponentType.Image},
        {"_Btn_" ,ComponentType.Button},
        {"_NBtn_" ,ComponentType.NormalBtn},
        {"_TxtP_",ComponentType.TextMesh},
        {"_Trs_",ComponentType.Transform},
        {"_Com_",ComponentType.ComponentSetter},
        {"_Maker_",ComponentType.TypeMaker}
    };
    internal static bool FindRelatedPath(Transform child, Transform root, out string path)
    {
        var current = child;
        List<string> pathList = new List<string>();
        int counter = 0;
        while (current != null && counter <= 10)
        {
            pathList.Add(current.gameObject.name);
            if (current == root)
            {
                path = ConvertListToPath(pathList);
                return true;
            }
            current = current.parent;
            counter++;
        }
        path = null;
        return false;
    }
    private static string ConvertListToPath(System.Collections.Generic.List<string> pathList)
    {
        string result = "";
        for (int index = pathList.Count - 2; index >= 0; index--)
        {
            if (index > 0)
            {
                result += pathList[index] + "/" ;
            }
            else
            {
                result += pathList[index];
            }
        }
        return result;
    }
}
public enum ComponentType
{ 
    None = 0,
    Text = 1,
    Image = 2,
    Button = 3,
    NormalBtn = 4,
    TextMesh = 5,
    Transform = 6,
    ComponentSetter = 7,
    TypeMaker = 8,
}
public class ComponentData
{
    public string Path;
    public virtual ComponentType ComType {get{ return ComponentType.None; } }
    public ComponentInfo ComInfo  = null;
    public ComponentData() { }
    public ComponentData(string pPath, ComponentInfo pComponentInfo) {
        Path = pPath;
        ComInfo = pComponentInfo;
    }
    public virtual void OnLoadComponent(Component pComponent, ComponentInfo pComInfo)
    {
       
    }
}
