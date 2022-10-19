using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SpriteAnimationAsset : BaseAsset
{
    public AniNameType aniName;
    public int Prox;

    public override BaseBehaviour GetBehaviour(PlayableGraph graph, GameObject go)
    {
        var baseBehaviour = new SpriteAnimationBehaviour();
        baseBehaviour.target_ = target_.GetGo(graph, go,out baseBehaviour.parent_);
        baseBehaviour.aniName = aniName;
        baseBehaviour.Prox = Prox;
        return baseBehaviour;
    }
}
