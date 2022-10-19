using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flow
{
    private Dictionary<int, Step> steps = new Dictionary<int, Step>();
    private int curStep = 0;
    private int maxStep = 0;
    private string flowName = "NoneName";
    public Flow(string flowName)
    {
        this.flowName = flowName;
    }
    public void RunAllStep(System.Action<bool> onFinish) {
        Debug.Log($"flowName {this.flowName} start run >>");
        curStep = 0;
        LoopRunAllStep(onFinish);
    }
    private void LoopRunAllStep(System.Action<bool> onFinish)
    {
        curStep++;
        if (curStep > maxStep)
        {
            Debug.Log($"flowName {this.flowName}finish result is : {true}");
            onFinish?.Invoke(true);
        }
        else {
            Step step;
            if (steps.TryGetValue(curStep, out step))
            {
                step.Execute((result,nextStepId) => {
                    Debug.Log($"flowName {this.flowName}step resultis{result }step id is {step.stepId}");
                    if (result == StepResult.Next)
                        LoopRunAllStep(onFinish);
                    else if (result == StepResult.Stop)
                    {
                        onFinish?.Invoke(false);
                        Debug.Log($"flowName {this.flowName}finish result is : {false}");
                    }
                    else if (result == StepResult.Jump)
                    {
                        curStep = nextStepId--;
                        LoopRunAllStep(onFinish);
                    }
                });
            }
            else
            {
                LoopRunAllStep(onFinish);
            }
        }
    }

    public void AddStep(int stepId, System.Action<System.Action<StepResult, int>> action)
    { 
        if (!steps.ContainsKey(stepId))
        {
            var step = new Step(stepId, action);
            steps.Add(stepId, step);
        }
        if(stepId > maxStep)
            maxStep = stepId;
    }
    public void AddStepAction(StepAction stepAction) {

        AddStep(stepAction.StepId,stepAction.GetExcureAction());
    }
}
