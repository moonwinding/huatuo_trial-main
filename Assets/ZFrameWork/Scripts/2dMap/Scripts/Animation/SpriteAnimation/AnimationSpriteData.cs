using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creat/AnimationSpriteData")]
public class AnimationSpriteData : ScriptableObject
{
    public AniNameType type;
    public int m_MinSpeed;
    public int m_MaxSpeed;
    public Sprite[] m_AnimatedSprites;
    public bool flipX;
    public bool flipY;
    
    public AnimationSpriteRunData GetRunData() {
        var runData = new AnimationSpriteRunData();
        runData.type = this.type;
        runData.m_AnimatedSprites = this.m_AnimatedSprites;
        runData.m_MinSpeed = this.m_MinSpeed;
        runData.m_MaxSpeed = this.m_MaxSpeed;
        runData.flipX = this.flipX;
        runData.flipY = this.flipY;
        return runData;
    }
}
public class AnimationSpriteRunData {

    public AniNameType type;
    public int m_MinSpeed;
    public int m_MaxSpeed;
    public Sprite[] m_AnimatedSprites;
    public bool flipX;
    public bool flipY;

    private int index_ = 0;
    public Sprite GetSprite()
    {
        return m_AnimatedSprites[index_];
    }
    public Sprite GetSpriteProgress(float pProgress) {
        int index = (int)(m_AnimatedSprites.Length * pProgress);
        if (index >= m_AnimatedSprites.Length)
            index = m_AnimatedSprites.Length - 1;
        return m_AnimatedSprites[index];
    }
    public void OnReset()
    {
        index_ = 0;
    }
    public bool OnSpriteShow()
    {
        index_++;
        if (index_ >= m_AnimatedSprites.Length)
        {
            index_ = 0;
            return true;
        }
        return false;
    }
}

