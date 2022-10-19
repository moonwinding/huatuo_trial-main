using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemsPanel : UIBase
{
    public TypeMaker typeMaker;
    //public ThingGroup thingGroup;
    public override void OnInit(){
        
    }
    public override void OnForward(){
        //var things = thingGroup.Things;
        //typeMaker.MakerByCount(things.Count,(oGo,oIndex)=> {
        //    var componentSetter = oGo.GetComponent<ComponentSetter>();
        //    var thing = things[oIndex -1];
        //    componentSetter.SetOneComponentData(new ImageData("icon", new ComponentInfo(){
        //        type = "sprite",
        //        objValue = thing.Icon

        //    }));
        //    componentSetter.SetOneComponentData(new TextData("count", new ComponentInfo() {
        //        type = "Text",
        //        strValue = "x" + thing.Count
        //    }));
        //});
    }
    public override void DoDestroy()
    {

    }
}
