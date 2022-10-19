using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creat/NewStoryBook")]
public class StoryBookSetter : ScriptableObject
{
    public List<StorySetter> storys;

    public StoryBook GetBook() { 
        var result = new StoryBook();
        for(var i = 0; i < storys.Count; i++) { 
            var story = storys[i];
            var storyBase = story.GetStory();
            result.storyMap.Add(storyBase.StoryId, storyBase);
        }
        return result;
    }
}
