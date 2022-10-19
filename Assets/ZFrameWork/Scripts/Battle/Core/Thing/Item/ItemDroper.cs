using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDroper : MonoBehaviour
{
    public ThingGroup thingGroup;
    private SpriteRenderer render;
    public void SetSprite(Sprite pSprite,int pLayer)
    {
        if (render == null)
            render = this.gameObject.GetComponent<SpriteRenderer>();
        render.sprite = pSprite;
        //render.sortingLayerID = pLayer;
    }
    private DropPanelCtrl dropPanelCtrl;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (dropPanelCtrl != null)
            return;
        var unitTag = collision.gameObject.GetComponent<UnitTag>();
        if (unitTag != null && unitTag.Id == BattleField.Instance.GetPlayerUnit().Id) {
            dropPanelCtrl = new DropPanelCtrl();
            dropPanelCtrl.SetDropData(thingGroup, ()=> {
                UICtrlManager.CloseTopBaseUI();
                AssetManager.Instance.FreeGameObject(this.gameObject);
                dropPanelCtrl = null;
            },()=> {
                UICtrlManager.CloseTopBaseUI();
                dropPanelCtrl = null;
            });
            UICtrlManager.OpenBaseUI(dropPanelCtrl,null);
        }
    }
}
