using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPanelCtrl : UICtrlBase
{
    protected override string Key { get { return "XPanelCtrl"; } }
    protected override string Path { get { return "xx.prefab"; } }

    protected override void OnInit()
    {
//SetComponentDatas
        SetFullCommonentData(fullData);
    }
    protected override void OnForward()
    {
        SetFullCommonentData(fullData);
    }
}
