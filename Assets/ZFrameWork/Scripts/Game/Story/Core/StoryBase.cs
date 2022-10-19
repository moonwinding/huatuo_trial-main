using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryBase
{
    public int StoryId;
    public List<StoryStepBase> steps = new List<StoryStepBase>();

    public StoryStepBase GetStep(int pStepId) {
        for (var i = 0; i < steps.Count; i++) {
            if (steps[i].stepId == pStepId)
            {
                return steps[i];
            }
        }
        return null;
    }
    public StoryStepBase GetNextStep(int pStepId) {
        for (var i = 0; i < steps.Count; i++)
        {
            if (steps[i].stepId > pStepId)
            {
                return steps[i];
            }
        }
        return null;
    }
}

