using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PositionSetAsset : BaseAsset
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public bool needSetPos;
    public bool needSetRot;
    public bool needSetScale;

    public override BaseBehaviour GetBehaviour(PlayableGraph graph, GameObject go)
    {
        var baseBehaviour = new PositionSetBehaviour();
        baseBehaviour.target_ = target_.GetGo(graph, go, out baseBehaviour.parent_);
        baseBehaviour.position = position;
        baseBehaviour.rotation = rotation;
        baseBehaviour.scale = scale;

        baseBehaviour.needSetPos = needSetPos;
        baseBehaviour.needSetRot = needSetRot;
        baseBehaviour.needSetScale = needSetScale;

        return baseBehaviour;
    }

}
