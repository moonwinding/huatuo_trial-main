using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExt
{

    //public static bool Overlaps(this RectTransform a, RectTransform b)
    //{
    //    return a.WorldRect().Overlaps(b.WorldRect());
    //}


    /// <summary>
    /// 
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <param name="pos">世界坐标的position</param>
    /// <returns></returns>
    public static Rect WorldRect2(this RectTransform rectTransform, Vector3 pos)
    {
        Rect rect = new Rect();
        Vector2 sizeDelta = rectTransform.sizeDelta;
        float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
        float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

        Vector3 position = pos;

        rect.x = position.x - rectTransformWidth * rectTransform.pivot.x;
        rect.y = position.y - rectTransformHeight * rectTransform.pivot.y;
        rect.width = rectTransformWidth;
        rect.height = rectTransformHeight;

        return rect;
    }
}
