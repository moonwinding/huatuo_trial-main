using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class StoryStepBase
{
    public int stepId;
    public PlayableAsset timeLineAsset;//���鲽��ʹ�õ�TimeLine ��Դ

    public System.Action onFinish;
    public void ResetAndPlay(PlayableDirector pPlayableDir,System.Action pOnFinish) {
        onFinish = pOnFinish;
        if (pPlayableDir != null && timeLineAsset != null) {
            pPlayableDir.playOnAwake = false;
            pPlayableDir.playableAsset = timeLineAsset;
            pPlayableDir.stopped -= OnFinish;
            pPlayableDir.stopped += OnFinish;
            pPlayableDir.Play();
        }
        else {
            Debug.LogError($"StoryStepBase ResetAndPlay Plable is nil or playable asset is nil!");   
        }
    }
    private void OnFinish(PlayableDirector pPlayableDirector) {
        onFinish?.Invoke();
    }
}
