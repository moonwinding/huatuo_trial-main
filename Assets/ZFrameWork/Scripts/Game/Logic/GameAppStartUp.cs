using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAppStartUp 
{
    public static void StartUp() {
        Debug.Log("GameAppStartUp StartUp >>");
        GameSceneManager.Init();
        GameSceneManager.EnterScene(GameSceneType.Login, () => {
            Debug.Log("Enter Loginc Scene Success  >>");
        });
    }
}
