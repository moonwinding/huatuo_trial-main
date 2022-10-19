using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInput : MonoBehaviour
{
    private UnitController controller;
    private Vector3 move;
    private BagPanelCtrl bagPanelCtrl;
    void Start(){
        controller = this.gameObject.GetComponent<UnitController>();
    }
    void Update(){
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(KeyCode.J)){controller.Atk();}    
        if(controller != null)
            controller.SetMoveDir(move);
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            OnClickBtn(KeyCode.Alpha1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            var playerData = BattleField.Instance.GetPlayerData();
            playerData.OnUseItem(1,1);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (bagPanelCtrl == null)
            {
                bagPanelCtrl = new BagPanelCtrl();
                bagPanelCtrl.SetBagPanel(() =>
                {
                    bagPanelCtrl = null;
                });
                UICtrlManager.OpenBaseUI(bagPanelCtrl, null);
            }
            else {
                UICtrlManager.CloseTopBaseUI();
                bagPanelCtrl = null;
            }
        }
    }
    private void OnClickBtn(KeyCode pKeyCode) {
        BattleField.Instance.OnClickKeyCode(pKeyCode, controller.unitId);
    }
}
