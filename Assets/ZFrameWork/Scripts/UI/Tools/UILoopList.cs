using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoopList : MonoBehaviour
{
    public GameObject cell;
    public int TestCount = 20;
    private IList m_Datas;
    private ScrollRect m_ScrollRect;
    private GridLayoutGroup group;
    private System.Action<GameObject, object> updateItemFunc;
    private RectTransform showRect;
    private Rect showWorldRect;

    private int showR;
    private int showC;
    private int allR;
    private int allC;

    private int showCount;
    private int allDataCount;
    private List<GameObject> freeGos = new List<GameObject>();
    private List<GameObject> allGos = new List<GameObject>();
    private List<Cell> cells = new List<Cell>();
    private List<Cell> removeCells = new List<Cell>();
    private GameObject hideRoot;
    private RectTransform conent;
    //private bool IsOnDrag = false;
    void Awake()
    {
        m_ScrollRect = GetComponentInParent<ScrollRect>();
        showRect = m_ScrollRect.gameObject.GetComponent<RectTransform>();
        conent = this.gameObject.GetComponent<RectTransform>();
        showWorldRect = showRect.WorldRect();
        group = GetComponent<GridLayoutGroup>();
        if (hideRoot == null)
            hideRoot = new GameObject("Hide");
        hideRoot.transform.SetParent(showRect.transform);
       // m_ScrollRect.onValueChanged.RemoveAllListeners();
        //m_ScrollRect.onValueChanged.AddListener(OnValueChange) ;
    }
    private void OnValueChange(Vector2 Pos)
    {
        UpdateShow();
    }
    public void SetData(List<object> pDatas,System.Action<GameObject,object> pUpdateFunc) {
        m_Datas = pDatas;
        updateItemFunc = pUpdateFunc;
        allDataCount = pDatas.Count;
        if (group.constraint == GridLayoutGroup.Constraint.FixedColumnCount){
            allC = group.constraintCount;
            showC = allC;
            allR = Mathf.FloorToInt(allDataCount / allC);
            showR = Mathf.FloorToInt(showRect.rect.height / (group.cellSize.y + group.spacing.y)) + 2;
            conent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, allR * group.cellSize.y);
        }
        else {
            allR = group.constraintCount;
            showR = allR;
            allC = Mathf.FloorToInt(allDataCount / allR);
            showC = Mathf.FloorToInt(showRect.rect.width / (group.cellSize.x + group.spacing.x)) + 2;
            conent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, allC * group.cellSize.x);
        }
        showCount = showR * showC;
        if (showCount > allGos.Count) {
            for (var i = allGos.Count; i < showCount; i++) {
                var go = GameObject.Instantiate<GameObject>(cell);
                go.name = "DataCell" +(i + 1);
                allGos.Add(go);
                freeGos.Add(go);
            }
        }

        if (allDataCount > cells.Count)
        {
            for (var i = cells.Count; i < allDataCount; i++)
            {
                var root = new GameObject("Cell" + (i + 1));
                root.transform.SetParent(this.transform);
                var rootRect = root.AddComponent<RectTransform>();
                var cell = new Cell(rootRect, showWorldRect, hideRoot, m_Datas[i],
                    updateItemFunc);
                cells.Add(cell);
            }
        }
        else if (allDataCount < cells.Count)
        {
            removeCells.Clear();
            for (var i = allDataCount; i < cells.Count; i++)
            {
                var cell = cells[i];
                var go = cell.OnHide();
                if(go != null)
                    freeGos.Add(go);
                removeCells.Add(cell);
            }
            for (var i = 0; i < removeCells.Count; i++) {
                var cell = removeCells[i];
                GameObject.Destroy(cell.rootRect.gameObject);
                cells.Remove(cell);
            }
        }
        UpdateShow();
        needSetData = true;
    }

    private void UpdateShow() {
        needSetData = false;
        for (var i = 0; i < cells.Count; i++) {
            var cell = cells[i];
            if (cell.NeedShow())
            {
                if (!cell.IsShow)
                {
                    if (freeGos.Count > 0) {
                        cell.OnShow(freeGos[0]);
                        freeGos.RemoveAt(0);
                    }
                }
            }
            else {
                if (cell.IsShow)
                {
                    var go = cell.OnHide();
                    freeGos.Add(go);
                }
            }
        }
    }

    private bool needSetData = false;
    void Update()
    {
        //if (!needSetData)
        //    return;
        UpdateShow();
    }

#if UNITY_EDITOR
    [Button("²âÊÔ×é¼þ")]
    private void Test()
    {
        var datas = new List<object>();
        for (var i = 0; i < TestCount; i++){
            datas.Add(i);
        }
        SetData(datas, (oGo, oData) =>
        {
            //Debug.LogError("oGo" + oGo.name);
            //Debug.LogError("oData" + oData.ToString());
        });
    }
#endif
}
public class Cell {
    private GameObject cell;
    private Rect maskWorldRect;
    public RectTransform rootRect;
    private GameObject hideRoot;
    private object data;
    private System.Action<GameObject,object> setFunc;
    public bool IsShow = false;
    public Cell(RectTransform pRootRect, Rect pMaskWorldRect,GameObject pHideRoot,object pData, 
        System.Action<GameObject, object> pSetFunc) {
        rootRect = pRootRect;
        hideRoot = pHideRoot;
        data = pData;
        setFunc = pSetFunc;
        maskWorldRect = pMaskWorldRect;
    }
    public bool NeedShow() {
        return maskWorldRect.Overlaps(rootRect.WorldRect(),true);
    }
    public void OnShow(GameObject pCell) {
        if (IsShow)
            return;
        IsShow = true;
        cell = pCell;
        cell.transform.SetParent(rootRect.transform, false);
        cell.transform.localPosition = Vector3.zero;
        cell.SetActive(true);
        setFunc?.Invoke(cell, data);
    }
    public GameObject OnHide() {
        if (!IsShow)
            return null;
        IsShow = false;
        cell.SetActive(false);
        cell.transform.SetParent(hideRoot.transform);
        return cell;
    }
}

public static class RectTransformExtensions{
    public static bool Overlaps(this RectTransform a, RectTransform b){
        return a.WorldRect().Overlaps(b.WorldRect());
    }
    public static bool Overlaps(this RectTransform a, RectTransform b, bool allowInverse){
        return a.WorldRect().Overlaps(b.WorldRect(), allowInverse);
    }
    public static Rect WorldRect(this RectTransform rectTransform){
        Vector2 sizeDelta = rectTransform.sizeDelta;
        float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
        float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;
        Vector3 position = rectTransform.position;
        return new Rect(
         position.x - rectTransformWidth * rectTransform.pivot.x,
         position.y - rectTransformHeight * rectTransform.pivot.y,
         rectTransformWidth,
         rectTransformHeight);
    }
}