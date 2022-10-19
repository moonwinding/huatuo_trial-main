using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryBook
{
    public Dictionary<int, StoryBase> storyMap = new Dictionary<int, StoryBase>();

    public StoryBase GetStoryBase(int pStoryId) {
        if (storyMap.TryGetValue(pStoryId, out StoryBase outStory)) {
            return outStory;
        }
        return null;
    }
}

