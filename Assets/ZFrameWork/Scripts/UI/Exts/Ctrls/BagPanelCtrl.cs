using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPanelCtrl : UICtrlBase
{
    protected override string Key { get { return "BagPanelCtrl"; } }
    protected override string Path { get { return "Assets/_ABs/LocalDontChange/UIPrefabs/BagPanel.prefab"; } }
    //private Thing selectThing;
    private System.Action onClose;
    public void SetBagPanel(System.Action pOnClose){
        onClose = pOnClose;
    }

    protected override void OnForward()
    {
        var uiBase_ = uiBase as BagPanel;
        //uiBase_.selectThing = selectThing;
        uiBase_.OnForward();
    }
    protected override void OnInit()
    {
        //this.AddEventListener("ClickThing", (obj) => {
        //    selectThing = obj as Thing;
        //    OnForward();
        //});
        //this.AddEventListener("ClickUseBtn", (obj) => {
        //    if (selectThing != null) {
        //        if (selectThing.Type == ThingType.Item)
        //        {
        //            selectThing.OnUse(1);
        //        }
        //        else if (selectThing.Type == ThingType.Equip) {
        //            var equip = selectThing as Equip;
        //            if (!equip.IsWear()){
        //                equip.WearUnit(BattleField.Instance.GetPlayerUnit());
        //            }
        //            else {
        //                equip.UnWear();
        //            }
        //        }
        //        OnForward();
        //    }
        //});
        //this.AddEventListener("ClickClose",(obj) => {
        //    UICtrlManager.CloseTopBaseUI();
        //    onClose?.Invoke();
        //});
    }
}
