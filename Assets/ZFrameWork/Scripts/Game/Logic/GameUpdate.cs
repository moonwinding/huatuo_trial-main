using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace ZFrameWork
{
    public class GameUpdate : MonoBehaviour
    {
        private int stepId = 0;
        private Flow updateFlow;
        private void Awake()
        {
            updateFlow = new Flow("GameUpdate");
            InitFlowData();
        }
        private int NewStepId()
        {
            stepId++;
            return stepId;
        }
        private void InitFlowData()
        {
            //string runtimePatchPath = string.Format(RUNTIME_PATCH_PATH_FORMAT, updateVersion);
            //初始化资源服务器
            //updateFlow.AddStep(NewStepId(), (oFinish) => {
            //    //0.init server info
            //    ResServer.ResServerTools.GerServerInfo(() => {
            //        oFinish.Invoke(StepResult.Next, 0);
            //    }, () => {
            //        oFinish.Invoke(StepResult.Stop, 0);
            //    });
            //});

            updateFlow.AddStepAction(new UpdateStep1());
            updateFlow.AddStepAction(new UpdateStepTest());
            //updateFlow.AddStepAction(new UpdateStep2());
            //updateFlow.AddStepAction(new UpdateStep3());
        }

        private void Start()
        {
            updateFlow.RunAllStep((isSuccess) =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("4_GameBattle");
            });
        }
    }
}
