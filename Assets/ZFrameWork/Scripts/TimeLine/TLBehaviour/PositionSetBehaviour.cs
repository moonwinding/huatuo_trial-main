using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSetBehaviour : BaseBehaviour
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public bool needSetPos;
    public bool needSetRot;
    public bool needSetScale;

    private bool isCanSend = false;
    protected override void OnProgress(float pProgress)
    {
        if (pProgress >= 0.1f)
        {
            if (isCanSend)
            {
                isCanSend = false;
                if (target_ != null) { 
                    if(needSetPos)
                        target_.transform.localPosition= position;
                    if (needSetRot)
                        target_.transform.localEulerAngles = rotation;
                    if (needSetScale)
                        target_.transform.localScale = scale;
                }
            }
        }
        if (pProgress < 0.1f)
        {
            isCanSend = true;
        }
    }
}
