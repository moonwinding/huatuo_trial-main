using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private SpriteAnimation spriteAnimator;
    private UnitAnimation unitAnimator;
    private void Awake()
    {
        spriteAnimator = this.gameObject.GetComponentInChildren<SpriteAnimation>();
        unitAnimator = this.gameObject.GetComponentInChildren<UnitAnimation>();
        if (this.gameObject.GetComponentInChildren<Animator>() != null && unitAnimator == null) {
            unitAnimator = this.gameObject.AddComponent<UnitAnimation>();
        }
    }

    public bool SetAniType(AniNameType pType, int pProx, bool pIsForce, System.Action pOnFinish)
    {
        if (spriteAnimator != null) { 
            return spriteAnimator.SetAniType(pType, pProx, pIsForce, pOnFinish);
        }
        else if(unitAnimator != null) {
            return unitAnimator.PlayAniByType(pType, pProx, pIsForce, pOnFinish);
        }
        return false;
    }
}
