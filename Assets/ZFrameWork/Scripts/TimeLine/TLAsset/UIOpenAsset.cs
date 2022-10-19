using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class UIOpenAsset : BaseAsset
{
    public string ctrlName;
    public bool isOpen;

    public override BaseBehaviour GetBehaviour(PlayableGraph graph, GameObject go)
    {
        var baseBehaviour = new UIOpenBehaviour();
        baseBehaviour.ctrlName = ctrlName;
        baseBehaviour.isOpen = isOpen;
        return baseBehaviour;
    }
}
