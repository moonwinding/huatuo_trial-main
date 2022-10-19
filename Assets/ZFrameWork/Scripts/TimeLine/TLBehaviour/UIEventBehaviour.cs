using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIEventType { 
    None =0,
    Text =1,
}

public class UIEventBehaviour: BaseBehaviour
{
    public UIEventType type = UIEventType.None;
    public string info = "";
    private bool isCanSend = false;
    protected override void OnProgress(float pProgress)
    {
        if (pProgress >= 0.5f)
        {
            if (isCanSend)
            {
               
                isCanSend = false;
                EventG.UIEvent.SendMessage(type, info);
            }
        }
        if (pProgress < 0.5f)
        {
            isCanSend = true;
        }
    }
}
