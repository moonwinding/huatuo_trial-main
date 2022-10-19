using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ComponentInfo{
    public string type;
    public string strValue;
    public int intValue;
    public float floatValue;
    public System.Action acValue;
    public System.Action<GameObject, int> goIntAcValue;
    public Vector3 vec3Value;
    public Object objValue;
    public ComponentInfo() { }
}

public class ComponentSetterFullData
{
    private Dictionary<int, ComponentData> componentDatas = new Dictionary<int, ComponentData>();
    public void AddComponentData(int pComId, ComponentData pComponentData)
    {
        if (!componentDatas.ContainsKey(pComId))
            componentDatas.Add(pComId, pComponentData);
        else
            componentDatas[pComId] = pComponentData;
    }
    public List<ComponentData> GetComponetnDatas()
    {
        var reslut = new List<ComponentData>();
        foreach (var temp in componentDatas)
        {
            reslut.Add(temp.Value);
        }
        return reslut;
    }
    public ComponentData GetComponent(int pComId)
    {
        ComponentData result;
        componentDatas.TryGetValue(pComId, out result);
        return result;
    }
}

public class TypeMakerData : ComponentData
{
    public override ComponentType ComType { get { return ComponentType.TypeMaker; } }
    public TypeMakerData(string pPath, int pCount,System.Action<GameObject,int> pSetFunc)
    {
        Path = pPath;
        ComInfo = new ComponentInfo() {intValue = pCount, goIntAcValue = pSetFunc };
    }
    public override void OnLoadComponent(Component pComponent, ComponentInfo pComInfo)
    {
        var com = pComponent as TypeMaker;
        com.MakerByCount(pComInfo.intValue, (oGo,oIndex)=> {
            pComInfo.goIntAcValue.Invoke(oGo, oIndex);
        });
    }
}

public class ComponentSetterData : ComponentData
{
    private ComponentSetterFullData data;
    public override ComponentType ComType { get { return ComponentType.ComponentSetter; } }
    public ComponentSetterData(string pPath, ComponentSetterFullData pData){
        Path = pPath;
        data = pData;
    }
    public override void OnLoadComponent(Component pComponent, ComponentInfo pComInfo)
    {
        var com = pComponent as ComponentSetter;
        com.SetComponentDataByFullData(data);
    }
}

public class ButtonData : ComponentData
{
    private System.Action onClick;
    public override ComponentType ComType { get { return ComponentType.Button; } }
    public ButtonData(string pPath,System.Action pClick) {
        Path = pPath;
        ComInfo = new ComponentInfo() { acValue = pClick };
    }
    public ButtonData(string pPath, ComponentInfo pComponentInfo)
    {
        Path = pPath;
        ComInfo = pComponentInfo;
    }
    public override void OnLoadComponent(Component pComponent, ComponentInfo pComInfo)
    {
        onClick = pComInfo.acValue;
        var com = pComponent as Button;
        com.onClick.RemoveAllListeners();
        com.onClick.AddListener(() => { 
            onClick.Invoke();
        });}
}

public class ImageData : ComponentData{
    public override ComponentType ComType { get { return ComponentType.Image; } }
    //public ImageData(string pPath,Sprite pSprite) {
    //    Path = pPath;
    //    ComInfo = new ComponentInfo() { type = "Image", objValue = pSprite };
    //}
    public ImageData(string pPath, float pFillAmount)
    {
        Path = pPath;
        ComInfo = new ComponentInfo() { type = "Fill", floatValue = pFillAmount };
    }
    public ImageData(string pPath, ComponentInfo pComponentInfo)
    {
        Path = pPath;
        ComInfo = pComponentInfo;
    }
    public override void OnLoadComponent(Component pComponent,ComponentInfo pComInfo)
    {
        if (pComInfo == null)
            return;
        if (pComInfo.type == "Image"){
            if (pComponent != null)
            {
                var com = pComponent as Image;
                com.sprite = pComInfo.objValue as Sprite;
            }
        }
        else if (pComInfo.type == "Fill"){
            var com = pComponent as Image;
            com.fillAmount = pComInfo.floatValue;
        }
    }
}
public class NormalBtnData : ComponentData{
    public override ComponentType ComType { get { return ComponentType.NormalBtn; } }
    public NormalBtnData(string pPath, int pIndex) {
        Path = pPath;
        ComInfo = new ComponentInfo() { intValue = pIndex };
    }
    public NormalBtnData(string pPath, ComponentInfo pComponentInfo)
    {
        Path = pPath;
        ComInfo = pComponentInfo;
    }
    public override void OnLoadComponent(Component pComponent, ComponentInfo pComInfo)
    {
        var com = pComponent as NormalBtn;
        com.Index = pComInfo.intValue;
    }
}

public class TextData : ComponentData{

    public override ComponentType ComType { get { return ComponentType.Text; } }
    public TextData(string pPath, string pText)
    {
        Path = pPath;
        ComInfo = new ComponentInfo() { type = "Text",strValue = pText };
    }
    public TextData(string pPath, ComponentInfo pComponentInfo)
    {
        Path = pPath;
        ComInfo = pComponentInfo;
    }
    public override void OnLoadComponent(Component pComponent, ComponentInfo pComInfo)
    {
        var com = pComponent as Text;
        if (pComInfo.type == "Text")
            com.text = pComInfo.strValue;
    }
}

public class TextMeshProData : ComponentData{
   
    public override ComponentType ComType { get { return ComponentType.TextMesh; } }
    public TextMeshProData(string pPath, string pText)
    {
        Path = pPath;
        ComInfo = new ComponentInfo() { type = "Text", strValue = pText };
    }
    public TextMeshProData(string pPath, ComponentInfo pComponentInfo)
    {
        Path = pPath;
        ComInfo = pComponentInfo;
    }
    public override void OnLoadComponent(Component pComponent,ComponentInfo pComInfo){
        
        var com = pComponent as TMPro.TextMeshProUGUI;
        if(pComInfo.type == "Text")
            com.text = pComInfo.strValue;
    }
}

public class TransformData : ComponentData {
    public override ComponentType ComType { get { return ComponentType.Transform; } }
    public TransformData(string pPath,Vector3 pPos,string pType = "localPos") {
        Path = pPath;
        ComInfo = new ComponentInfo() { type = pType,vec3Value = pPos };
    }
    public TransformData(string pPath, ComponentInfo pComponentInfo)
    {
        Path = pPath;
        ComInfo = pComponentInfo;
    }
    public override void OnLoadComponent(Component pComponent, ComponentInfo pComInfo)
    {
        var com = pComponent as Transform;
        if (pComInfo.type == "localPos")
            com.localPosition = pComInfo.vec3Value;
        else if (pComInfo.type == "worldPos") 
            com.position = pComInfo.vec3Value;
        else if(pComInfo.type =="localRot")
            com.localEulerAngles = pComInfo.vec3Value;
        else if (pComInfo.type == "worldRot")
            com.eulerAngles = pComInfo.vec3Value;
        else if (pComInfo.type == "scale")
            com.localScale = pComInfo.vec3Value;
    }
}
