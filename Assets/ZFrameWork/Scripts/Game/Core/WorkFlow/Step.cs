using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//该步的执行结果
public enum StepResult
{ 
    Next = 0,   //执行结束，进行下一步执行
    Stop = 1,   //执行结束，并终端后续所有步骤的执行 跳出工作流程
    Jump = 2,   //执行结束, 跳到指定另外一个步骤进行执行
}
public class Step
{
    public int stepId;
    private System.Action<System.Action<StepResult,int>> action;
    public Step(int stepId,System.Action<System.Action<StepResult, int>> action) {
        this.stepId = stepId;
        this.action = action;
    }
    public void Execute(System.Action<StepResult, int> finish) {
        action?.Invoke(finish);
    }
}
