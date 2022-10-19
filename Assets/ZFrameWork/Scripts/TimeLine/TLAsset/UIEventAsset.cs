using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class UIEventAsset : BaseAsset
{
    public UIEventType type = UIEventType.None;
    public string info = "";

    public override BaseBehaviour GetBehaviour(PlayableGraph graph, GameObject go)
    {
        var baseBehaviour = new UIEventBehaviour();
        baseBehaviour.type = type;
        baseBehaviour.info = info;

        return baseBehaviour;
    }
}
