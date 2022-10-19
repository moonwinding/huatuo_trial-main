using UnityEditor;
using UnityEngine;

public class AnimatorBehaviour : BaseBehaviour
{
    public string aniName;
	public int layer;
    private Animator animator;
	private float aniLength;
	private float passTime;
	private AnimationClip clip;
    protected override void OnProgress(float pProgress)
    {
		if (animator == null)
		{
			animator = target_.GetComponentInChildren<Animator>();
			aniLength = AnimatorExt.GetClipLength(animator, aniName,out clip);
			//if (animator == null) {
			//	//Debug.Log($"animator is nil plz check name {aniName}");
			//}
		}
		passTime = aniLength * pProgress;
		if (Application.isPlaying)
		{
			if (animator != null)
			{
				animator.speed = 0f;
				animator.Play(aniName, layer, passTime);
			}
		}
		else
		{
#if UNITY_EDITOR
			if (animator != null)
			{
				if (clip != null)
				{
					AnimationMode.SampleAnimationClip(animator.gameObject, clip, passTime);
				}
				else
				{
					Debug.Log($"clip is nil plz check name {aniName}");
				}
			}
#endif
		}
	}
}

///获取动画状态机animator的动画clip的播放持续时长
public static class AnimatorExt
{
	public static float GetClipLength(Animator animator, string clip,out AnimationClip oClip)
	{
		oClip = null;
		if (null == animator || string.IsNullOrEmpty(clip) || null == animator.runtimeAnimatorController)
			return 0;
		RuntimeAnimatorController ac = animator.runtimeAnimatorController;
		AnimationClip[] tAnimationClips = ac.animationClips;
		if (null == tAnimationClips || tAnimationClips.Length <= 0) return 0;
		AnimationClip tAnimationClip;
		for (int i = 0, tLen = tAnimationClips.Length; i < tLen; i++)
		{
			tAnimationClip = ac.animationClips[i];
			if (null != tAnimationClip && tAnimationClip.name == clip)
			{
				oClip = tAnimationClip;
				return tAnimationClip.length;
			}
				
		}
		return 0;
	}
}
