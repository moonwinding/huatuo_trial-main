using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameSceneType { 
    None =0,
    Login =1,//登录场景
    Game =2,//游戏场景
    Battle =3,//战斗场景

}
public class GameSceneBase
{
    private bool isInit = false;

    public virtual GameSceneType SceneType { get { return GameSceneType.None; } }
    public GameSceneBase() { 
        
    }
    protected void DoInit() {
        if (isInit)
            return;
        OnInt();
    }
    public virtual void OnInt() { 
        
    }
    public virtual void OnEnter(System.Action onFinish) { 
        
    }
    public virtual void OnExit(System.Action onFinish) { 
        
    }
}
