using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ZFrameWork {
    public class UILoopCell
    {
        public Rect rootRect;
        private GameObject cellGo;
        private int cellType;
        private Rect maskWorldRect;
        private Vector3 pos;
        private float width;
        private float height;
        private GameObject hideRoot;
        private object data;
        private System.Action<GameObject, object, int, int> setFunc;

        private RectTransform showRectTransform;
        private int cellIndex;
        public bool IsShow = false;

        private float transfromScalex;
        private float transformScaley;

        private bool needSetChildWidth = false;

        public void SetNeedSetChildWidth(bool pIsNeed)
        {
            needSetChildWidth = pIsNeed;
        }

        public UILoopCell(RectTransform pShowRectTransform, Rect pMaskWorldRect, GameObject pHideRoot, object pData,
            System.Action<GameObject, object, int, int> pSetFunc, int pIndex)
        {
            showRectTransform = pShowRectTransform;
            hideRoot = pHideRoot;
            data = pData;
            setFunc = pSetFunc;
            maskWorldRect = pMaskWorldRect;
            cellIndex = pIndex;
            transfromScalex = pShowRectTransform.lossyScale.x;
            transformScaley = pShowRectTransform.lossyScale.y;
        }
        public void SetMaskExHeight(float pExHeight)
        {
            maskWorldRect.height += pExHeight;
        }
        public void SetNewWorldMaskRect(Rect pMaskWorldRect)
        {
            maskWorldRect = pMaskWorldRect;
        }
        public void SetNewData(object pData)
        {
            data = pData;
        }
        public void SetPosInfo(Vector3 pPos, float pWidth, float pHeight)
        {
            pPos = pos;
            width = pWidth;
            height = pHeight;
            rootRect = new Rect(pPos.x, pPos.y, width * transfromScalex, height * transformScaley);
        }
        public float GetHeight() { return height; }
        public Vector3 GetPos() { return pos; }
        public void SetPosOnly(Vector3 pPos)
        {
            pos = pPos;
            rootRect = new Rect(pPos.x, pPos.y, width * transfromScalex, height * transformScaley);
        }
        public bool NeedShow()
        {
            rootRect.position = showRectTransform.position + (pos - Vector3.up * height) * transformScaley;
            var isNeedShow = maskWorldRect.Overlaps(rootRect, true);
            return isNeedShow;
        }
        private System.Action onHeightChange;
        private bool isNeedUpdateHeight = false;

        public void SetDynamicHeightInfo(bool pIsNeedHeightUpdate, System.Action pOnHeightChange)
        {
            isNeedUpdateHeight = pIsNeedHeightUpdate;
            onHeightChange = pOnHeightChange;
        }
        private void RefreshCellHeight(RectTransform pCellRect)
        {
            if (!isNeedUpdateHeight)
                return;
            isNeedUpdateHeight = false;
            RefreshCellHeightRealy(pCellRect);
        }
        private void RefreshCellHeightRealy(RectTransform pCellRect)
        {
            var cellRect = pCellRect;
            var maxY = cellRect.rect.yMax;
            var minY = cellRect.rect.yMin;
            GetRectMinYAndMaxY(pCellRect, out float oHeight, out float oWidth);
            if (height != oHeight)
            {
                height = oHeight;
                onHeightChange?.Invoke();
            }
        }
        private void GetRectMinYAndMaxY(RectTransform pRect, out float outHeight, out float outWidth)
        {
            outHeight = 0f;
            outWidth = 0f;
            float minY = 99999;
            float maxY = -99999;

            float minX = 99999;
            float maxX = -99999;

            var rects = pRect.gameObject.GetComponentsInChildren<RectTransform>();
            if (rects != null)
            {
                for (var i = 0; i < rects.Length; i++)
                {
                    var rect = rects[i];
                    if (rect == pRect)
                    {
                        continue;
                    }
                    var maxY_ = rect.rect.yMax;
                    var minY_ = rect.rect.yMin;
                    var maxX_ = rect.rect.xMax;
                    var minX_ = rect.rect.xMin;
                    if (maxX_ > maxX) { maxX = maxX_; }
                    if (minX_ < minX) { minX = minX_; }
                    if (maxY_ > maxY) { maxY = maxY_; }
                    if (minY_ < minY) { minY = minY_; }
                }
            }
            outHeight = maxY - minY;
            outWidth = maxX - minX;
        }

        public void OnShow(GameObject pCellGo)
        {
            if (IsShow)
                return;
            IsShow = true;
            cellGo = pCellGo;
            cellGo.transform.SetParent(showRectTransform.transform, false);
            var cellRect = cellGo.transform as RectTransform;
            if (cellRect != null)
            {
                cellRect.transform.localPosition = pos;
                if (needSetChildWidth)
                {
                    cellRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, showRectTransform.rect.width);
                }
            }
            cellGo.transform.localScale = Vector3.one;
            cellGo.SetActive(true);
            setFunc?.Invoke(cellGo, data, cellIndex, cellType);
            RefreshCellHeight(cellRect);
        }
        public GameObject OnHide()
        {
            if (!IsShow)
                return null;
            IsShow = false;
            cellGo.SetActive(false);
            cellGo.transform.SetParent(hideRoot.transform);
            return cellGo;
        }
        public int GetCellType()
        {
            return cellType;
        }
        public void SetCellType(int pCellType)
        {
            cellType = pCellType;
        }
    }
}
public static class RectTransformEx {
    public static float transformScale = 0.01333333f;

    public static bool OverlapsEx(this RectTransform a,RectTransform b) {
        return a.WorldRectEx().Overlaps(b.WorldRectEx());
    }

    public static bool OverlapsEx(this RectTransform a, RectTransform b,bool allowInverse) {
        return a.WorldRectEx().Overlaps(b.WorldRectEx(), allowInverse);
    }

    public static Rect WorldRectEx(this RectTransform rectTransform) {
        float rectTransformWidth = rectTransform.rect.width * rectTransform.lossyScale.x;
        float rectTransformHeight = rectTransform.rect.height * rectTransform.lossyScale.y;
        Vector3 position = rectTransform.position;

        return new Rect(
            position.x - rectTransformWidth*rectTransform.pivot.x, 
            position.y - rectTransformHeight*rectTransform.pivot.y,
            rectTransformWidth, 
            rectTransformHeight);
    }
}

