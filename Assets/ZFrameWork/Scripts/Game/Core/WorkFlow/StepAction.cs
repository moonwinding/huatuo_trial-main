using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StepAction
{
    public virtual int StepId { get { return 1; } }
    public virtual System.Action<System.Action<StepResult, int>> GetExcureAction() {

        return null;
    }
    
}
