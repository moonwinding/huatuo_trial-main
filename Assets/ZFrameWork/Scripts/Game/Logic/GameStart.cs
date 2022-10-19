using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    private Flow startFlow;
    private void Awake()
    {
        startFlow = new Flow("StartFlow");
        InitStartFlow();
    }
    private void InitStartFlow()
    {
        //读取游戏设置
        startFlow.AddStep(1, (o) => {
            BetterStreamingAssets.Initialize();
            ZFrameWork.GameSettings.Init(() => {
                o.Invoke( StepResult.Next,0);
            });
        });
        //初始化SRDebug 
        startFlow.AddStep(2, (o) => {
            if (ZFrameWork.GameSettings.gameSettingConfig.IsDebug)
            {
                SRDebug.Init();
            }
            o.Invoke(StepResult.Next, 0);
        });
    }
    void Start()
    {
        startFlow.RunAllStep((oSuccess) => {
            if (ZFrameWork.GameSettings.gameSettingConfig.UseResServer)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("2_GameUpdate");
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("3_GameMain");
            }
        });
    }
}
