using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    public List<AnimationSpriteData> animations = new List<AnimationSpriteData>();
    private SpriteRenderer render;
    public AniNameType defaultType = AniNameType.Idle;
    public AniNameType type = AniNameType.None;
   
    private float animationSpeed = 1f;
    private Dictionary<AniNameType, AnimationSpriteRunData> animationMap 
        = new Dictionary<AniNameType, AnimationSpriteRunData>();
    private AnimationSpriteRunData nextData;
    //private SpriteAnimationType showType = SpriteAnimationType.None;
    private AnimationSpriteRunData showData;
    private float nextShowTime = 0f;
    private float passTime = 0f;
    private float cdTime = 1f;
    private System.Action onFinish;
    private int prox;
    // Start is called before the first frame update
    void Awake(){
        render = this.gameObject.GetComponent<SpriteRenderer>();
        foreach (var temp in animations) {
            if (!animationMap.ContainsKey(temp.type))
            {
                animationMap.Add(temp.type, temp.GetRunData());
            }
        }
        prox = 0;
        showData = null;
        type = AniNameType.None;
        SetAniType(defaultType, 1,true,null);
    }
    public void OnAnimationProgress(AniNameType pAniNameType,float pProgress) {
        DoInit();
        AnimationSpriteRunData data_ = null;
        if (animationMap.TryGetValue(pAniNameType, out data_))
        {
            var sprite = data_.GetSpriteProgress(pProgress);
            render.sprite = sprite;
        }
    }
    private bool isInit = false;
    public void DoInit()
    {
        //if (isInit)
        //    return;
        //isInit = true;
        render = this.gameObject.GetComponent<SpriteRenderer>();
        foreach (var temp in animations)
        {
            if (!animationMap.ContainsKey(temp.type))
            {
                animationMap.Add(temp.type, temp.GetRunData());
            }
        }
        prox = 0;
        showData = null;
        type = AniNameType.None;
    }
    public void Reset()
    {
        foreach (var temp in animationMap){
            temp.Value.OnReset();
        }
        prox = 0;
        showData = null;
        type = AniNameType.None;
        SetAniType(defaultType, 1, true, null);
    }
    public bool SetAniType(AniNameType pType, int pProx,bool pIsForce,System.Action pOnFinish) {
        if (!pIsForce  && pProx < prox)
            return false;
        if (IsPlayAning(pType))
            return false;
        type = pType;
        prox = pProx;
        onFinish = pOnFinish;
        if (animationMap.TryGetValue(type, out nextData))
        {
            animationSpeed = Random.Range(nextData.m_MinSpeed, nextData.m_MaxSpeed);
            cdTime = 1 / animationSpeed;
        }
        OnShow();
        return true;
    }
    private bool IsPlayAning(AniNameType pType) {
        return showData != null && showData.type == pType;
    }

    private void OnShow() {
        if (showData != nextData)
            nextData.OnReset();
        render.sprite = nextData.GetSprite();
        render.flipX = nextData.flipX;
        render.flipY = nextData.flipY;
        showData = nextData;
        var isFinish = nextData.OnSpriteShow();
        if (isFinish)
        {
            //showdata = null;
            onFinish?.Invoke();
            //onFinish = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        passTime += Time.deltaTime;
        if (passTime >= nextShowTime)
        {
            nextShowTime = passTime + cdTime;
            OnShow();
        }
    }
}
