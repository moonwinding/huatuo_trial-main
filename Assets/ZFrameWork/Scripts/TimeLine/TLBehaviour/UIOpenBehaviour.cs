using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIOpenBehaviour : BaseBehaviour
{
    public string ctrlName;
    public bool isOpen;

    private bool isCanRun = false;

    protected override void OnProgress(float pProgress)
    {
        if (pProgress >= 0.1f)
        {
            if (isCanRun)
            {
                if (isOpen)
                {
                    var type = Type.GetType(ctrlName);
                    var instance = System.Activator.CreateInstance(type);
                    var typeIns = instance as UICtrlBase;
                    UICtrlManager.OpenBaseUI(typeIns, null);
                }
                else {
                    var topCtrl = UICtrlManager.GetBaseTopCtrl();
                    if (topCtrl != null && topCtrl.GetKey() == ctrlName) {
                        UICtrlManager.CloseTopBaseUI();
                    }
                }
                isCanRun = false;
            }
        }
        if (pProgress < 0.1f)
        {
            isCanRun = true;
        }
    }
}
