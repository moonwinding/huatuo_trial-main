using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPanelCtrl : UICtrlBase
{
    protected override string Key { get { return "StoryPanelCtrl"; } }
    protected override string Path { get { return "Assets/_ABs/LocalDontChange/UIPrefabs/StoryPanel.prefab"; } }

    protected override void OnInit()
    {
        EventG.UIEvent.AddListener(UIEventType.Text, OnTextData);
    }
    private void OnTextData(object pInfo) {
        if (uiBase == null)
            return;
        //var uiBase_ = uiBase;
        var textData = new TextData("_LeftContent/_Txt_Left", pInfo as string);
        if (fullData == null)
        {
            fullData = new ComponentSetterFullData();
        }
        fullData.AddComponentData(1, textData);
        uiBase.SetFullCommonentData(fullData);
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        EventG.UIEvent.RemoveListener(UIEventType.Text, OnTextData);
    }

}
