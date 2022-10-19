using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneLogin : GameSceneBase
{
    public override GameSceneType SceneType { get { return GameSceneType.Login; } }

    private Flow loginFlow;
    public override void OnInt()
    {
        loginFlow = new Flow("Login");
        loginFlow.AddStep(1, (oCB) => {
            var loginPanelCtrl = new LoginPanelCtrl();
            UICtrlManager.OpenBaseUI(loginPanelCtrl, () => {
                oCB?.Invoke(StepResult.Next,1);
            });
        });

    }
    public override void OnEnter(Action onFinish)
    {
        loginFlow.RunAllStep((oSuccess) => {
            onFinish?.Invoke();
        });
    }
}
