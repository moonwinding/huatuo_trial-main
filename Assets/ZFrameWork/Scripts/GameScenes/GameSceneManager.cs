using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSceneManager
{

    public static Dictionary<GameSceneType, GameSceneBase> sceneMap = new Dictionary<GameSceneType, GameSceneBase>();
    private static GameSceneBase curScene;
    public static void Init() {
        sceneMap.Clear();
        var loginScene = new GameSceneLogin();
        loginScene.OnInt();
        sceneMap.Add(loginScene.SceneType,loginScene);

    }
    private static  GameSceneBase GetGameScene(GameSceneType SceneType) {
        sceneMap.TryGetValue(SceneType, out GameSceneBase scene);
        return scene;
    }
    private static void ExistCurScene(System.Action onFinish) {
        if (curScene != null)
        {
            curScene.OnExit(() =>
            {
                onFinish?.Invoke();
            });
        }
        else
        {
            onFinish?.Invoke();
        }
    }
    public static void EnterScene(GameSceneType SceneType,System.Action onFinish) {
        ExistCurScene(() => {
            curScene = null;
            var newScene = GetGameScene(SceneType);
            newScene.OnEnter(() => {
                curScene = newScene;
                onFinish?.Invoke();
            });
        });
    }
    
}
