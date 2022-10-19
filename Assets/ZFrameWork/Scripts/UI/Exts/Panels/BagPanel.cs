using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPanel : UIBase
{
    public TypeMaker typeMaker;
    public ComponentSetter curThing;

    //[HideInInspector]
    //public Thing selectThing;
    public override void OnForward()
    {
        //var playerData = BattleField.Instance.GetPlayerData();
        //var things = playerData.GetThings();
        //typeMaker.MakerByCount(things.Count, (oGo, oIndex) => {
        //    var componentSetter = oGo.GetComponent<ComponentSetter>();
        //    var thing = things[oIndex - 1];
        //    componentSetter.SetOneComponentData(new ImageData("icon", new ComponentInfo() { type = "Sprite",objValue = thing.Icon }));
        //    componentSetter.SetOneComponentData(new TextData("count", new ComponentInfo() { type = "Text", strValue = "x" + thing.Count }));
        //    componentSetter.SetOneComponentData(new TextData("name", new ComponentInfo() { type = "Text", strValue = thing.Name }));
        //    componentSetter.SetOneComponentData(new ButtonData("btn",()=> {
        //        SendMessageToCtrl("ClickThing", thing);
        //    }));
        //});
        //curThing.gameObject.SetActive(selectThing != null);
        //if (selectThing != null) {
        //    curThing.SetOneComponentData(new ImageData("icon", selectThing.Icon));
        //    curThing.SetOneComponentData(new TextData("count", "x" + selectThing.Count));
        //    curThing.SetOneComponentData(new TextData("name", selectThing.Name));
        //    curThing.SetOneComponentData(new ButtonData("btn", () => {
        //        SendMessageToCtrl("ClickUseBtn");
        //    }));
        //    if (selectThing.Type == ThingType.Item)
        //    {
        //        curThing.SetOneComponentData(new TextData("btn/btnText", "道具使用"));
        //    }
        //    else if (selectThing.Type == ThingType.Equip) {
        //        var equip = selectThing as Equip;
        //        if(equip.IsWear())
        //            curThing.SetOneComponentData(new TextData("btn/btnText", "卸下装备"));
        //        else
        //            curThing.SetOneComponentData(new TextData("btn/btnText", "穿戴装备"));
        //    }
        //}
    }
    public override void DoDestroy()
    {

    }
}
