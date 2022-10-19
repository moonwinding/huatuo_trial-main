using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creat/NewStoryStep")]
public class StoryStepSetter : ScriptableObject
{
    public StoryStepBase step;

    public StoryStepBase GetStep()
    {
        var step_ = new StoryStepBase();
        step_.stepId = step.stepId;
        step_.timeLineAsset = step.timeLineAsset;
        return step_;
    }
}
