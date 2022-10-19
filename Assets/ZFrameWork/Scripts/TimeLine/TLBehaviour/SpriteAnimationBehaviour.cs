using UnityEditor;
using UnityEngine;

public class SpriteAnimationBehaviour : BaseBehaviour
{
	public AniNameType aniName;
	public int Prox;
	private SpriteAnimation spriteAnimation;
		
	protected override void OnProgress(float pProgress)
	{
		if (spriteAnimation == null)
		{
			spriteAnimation = target_.GetComponentInChildren<SpriteAnimation>();
		}
		spriteAnimation.OnAnimationProgress(aniName, pProgress);
	}
}