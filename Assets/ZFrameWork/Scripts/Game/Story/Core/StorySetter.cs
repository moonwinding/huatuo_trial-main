using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creat/NewStory")]
public class StorySetter : ScriptableObject
{
    public int storyId;
    public List<StoryStepSetter> steps = new List<StoryStepSetter>();

    public StoryBase GetStory()
    {
        var result = new StoryBase();
        result.StoryId = this.storyId;
        for (var i = 0; i < this.steps.Count; i++)
        {
            result.steps.Add(this.steps[i].GetStep());
        }
        return result;
    }
}