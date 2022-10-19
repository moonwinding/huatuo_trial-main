using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPanelCtrl : UICtrlBase
{
    protected override string Key { get { return "DropPanelCtrl"; } }
    protected override string Path { get { return "Assets/_ABs/LocalDontChange/UIPrefabs/DropsPanel.prefab"; } }


    private ThingGroup thingGroup;
    private DropItemsPanel dropItemsPanel;
    private System.Action onPick;
    private System.Action onClose;
    public void SetDropData(ThingGroup pThingGroup, System.Action pOnPick, System.Action pOnClose)
    {
        thingGroup = pThingGroup;
        onPick = pOnPick;
        onClose = pOnClose;
    }
    protected override void OnForward(){
        var uiBase_ = uiBase as DropItemsPanel;
        //uiBase_.thingGroup = thingGroup;
    }
    protected override void OnInit()
    {
        this.AddEventListener("PickAllBtn", (obj) => {
            //BattleField.Instance.GetPlayerData().AddThingGroup(thingGroup);
            //onPick?.Invoke();
        });
        this.AddEventListener("CloseBtn", (obj) => {
            onClose?.Invoke();
        });
    }
}
