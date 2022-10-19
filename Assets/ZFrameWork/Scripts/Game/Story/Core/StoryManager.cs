using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StoryManager : MonoBehaviour
{
    public StoryBookSetter bookSetter;
    private StoryBook book;
    public int startStoryId;
    public PlayableDirector director;

    private static StoryManager instance;
    public static StoryManager Instance { get { return instance; } }
    private void Awake()
    {
        instance = this;
        book = bookSetter.GetBook();
        if (director == null) {
            director = this.gameObject.GetComponent<PlayableDirector>();
        }
    }
    private StoryBase curStory;
    private StoryStepBase curStep;
    private int curStepId;
    private void Start()
    {
        curStory = book.GetStoryBase(startStoryId);
        if (curStory != null) {
            LoopPlayCurStory(1, () => {
                Debug.LogError("on all finish!!!");
            });
        }
       
    }
    private void LoopPlayCurStory(int pCurStepId,System.Action pOnFinish) {
        curStep = curStory.GetStep(pCurStepId);
        if (curStep == null)
        {
            pOnFinish?.Invoke();
        }
        else {
            PlayOneStep(curStep, () => {
                pCurStepId++;
                LoopPlayCurStory(pCurStepId, pOnFinish);
            });
        }
    }
    private System.Action onOneFinish;
    private void PlayOneStep(StoryStepBase pStep,System.Action pOnOneFinish) {
        onOneFinish = pOnOneFinish;
        var timeLineAsset = pStep.timeLineAsset;
        director.playableAsset = timeLineAsset;
        director.stopped -= OnFinish;
        director.stopped += OnFinish;
        director.Play();
    }
    private void OnFinish(PlayableDirector pPlayableDirector) {
        onOneFinish?.Invoke();
    }
}
