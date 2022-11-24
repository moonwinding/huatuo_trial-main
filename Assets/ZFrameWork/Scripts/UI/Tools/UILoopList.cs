using NaughtyAttributes;
using SRF;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using ZFrameWork;

[System.Serializable]
public class UILoopCellGrid
{
    public bool UseDynimcSize = false;          //��̬��С,ʹ��Ԫ���Լ��Ĵ�С���и���
    public bool needTypeReset = false;          //��ǰһ�����ͺͱ����Ͳ�һ���Ļ���Ҫ�������ô���
    public Vector3 resetOffset = Vector3.zero;  //�������õ�λ�ò���
    public Vector3 mainOffset = Vector3.right;  //��Ҫ���˵�ƫ����
    public int MainOffCount = -1;               //��ƫ�ƶ��ٴν���һ�δ�ƫ��
    public Vector3 subOffPos = Vector3.down;    //��ƫ����
}
public class UILoopList : MonoBehaviour
{
    public List<GameObject> models = new List<GameObject>();                //����������ʾ��ģ���׼�б�
    public List<UILoopCellGrid> modelGrids = new List<UILoopCellGrid>();    //ÿ��ģ�͵����������б�

    public RectTransform minCellRect;
    public bool isUseChildHeight = false;                                   //ʹ���������Լ��ĸ߶�
    public bool NeedAutoSetChildWidth = false;                              //������ʾ����Ĵ�С������������Ŀ��
    public float ExScrollHeight = 0f;
    public float ExScrollWidth = 0f;
    public Vector2 ScrollOffset = Vector2.zero;
    public Vector3 startPos;
    public int setMaxShowCount = 0;

    private bool needDynamicSetSize = true;
    private List<int> cellTypes = new List<int>();
    private List<object> cellDatas = new List<object>();
    private ScrollRect mScrollRect;

    private LayoutGroup mLayoutGroup;
    private GridLayoutGroup gridGroup;
    private VerticalLayoutGroup verGroup;
    private HorizontalLayoutGroup horGroup;

    private System.Action<GameObject, object, int, int> updateItemFunc;
    private System.Action<GameObject, object, int, int, System.Action<float>> preSizeFunc;
    public bool needPreSize = false;//��ҪԤ�ȼ���Ԫ�صĴ�С
    private RectTransform showRect;
    private Rect showWorldRect;

    private int showR, showC, allR, allC = 1;
    private int showCount,allDataCount;
    private float totalHeight;

    private Dictionary<int, List<GameObject>> fressGos = new Dictionary<int, List<GameObject>>();
    private Dictionary<int, List<GameObject>> allGos = new Dictionary<int, List<GameObject>>();
    private List<UILoopCell> cells = new List<UILoopCell>();

    private GameObject hideRoot;
    private RectTransform conent;

    private bool isAwake = false;
    private Vector2 oriScrPos = Vector3.zero;
    private bool canForceUpdate = false;
    private bool updateAllcell = false;

    private Vector3 nextCellPos;
    private int nextCellType = -1;
    private int nextOffCount = 0;
    private string hideRootName = "HideRoot";

    private void Awake()
    {
        if (isAwake) return;isAwake = true;
        mScrollRect = GetComponentInParent<ScrollRect>();
        showRect = mScrollRect.gameObject.GetComponent<RectTransform>();
        conent = this.gameObject.GetComponent<RectTransform>();
        GetShowWorldRect();
        mLayoutGroup = GetComponent<LayoutGroup>();
        gridGroup = mLayoutGroup as GridLayoutGroup;
        verGroup = mLayoutGroup as VerticalLayoutGroup;
        horGroup = mLayoutGroup as HorizontalLayoutGroup;
        InitHideRoot();
    }
    private void InitHideRoot() {
        if (hideRoot == null) {
            var findHideRoot = showRect.Find(hideRootName);
            if (findHideRoot != null){
                hideRoot = findHideRoot.gameObject;
            }
            else {
                hideRoot = new GameObject(hideRootName);
                hideRoot.transform.SetParent(showRect.transform);
            }
        }
    }
    private void GetShowWorldRect() {
        showWorldRect = showRect.WorldRectEx();
        //����ʵ����ʾ�������һ���ļ������
        showWorldRect.height += ExScrollHeight;
        showWorldRect.width += ExScrollWidth;
        showWorldRect.position += ScrollOffset;
    }

    private void OnEnable(){
        GetShowWorldRect();
    }
    public UILoopCellGrid GetCellGrid(int pCellType) {
        return modelGrids[pCellType];
    }
    private bool IsDynamicModel(int pCellType) { 
        var grid = GetCellGrid(pCellType);
        return grid.UseDynimcSize;
    }
    private void InitGridInfo() {
        if (gridGroup != null)
        {
            if (gridGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
            {
                allC = gridGroup.constraintCount;
                showC = allC;
                allR = Mathf.FloorToInt(allDataCount / allC);
                showR = Mathf.FloorToInt(showRect.rect.height / (gridGroup.cellSize.y + gridGroup.spacing.y)) + 2;
            }
            else
            {
                allR = gridGroup.constraintCount;
                showR = allR;
                allC = Mathf.FloorToInt(allDataCount / allR);
                showC = Mathf.FloorToInt(showRect.rect.width / (gridGroup.cellSize.x + gridGroup.spacing.x)) + 2;
            }
        }
        else if (verGroup != null)
        {
            allC = 1;
            showC = allC;
            allR = Mathf.FloorToInt(allDataCount / allC);
            showR = Mathf.FloorToInt(showRect.rect.height / (minCellRect.sizeDelta.y + verGroup.spacing)) + 2;
        }
        else if (horGroup != null) {
            allR = 1;
            showR = allR;
            allC = Mathf.FloorToInt(allDataCount / allR);
            showC = Mathf.FloorToInt(showRect.rect.width / (minCellRect.sizeDelta.x + horGroup.spacing)) + 2;
        }
        showCount = showR * showC;
        if (setMaxShowCount > 0) {
            showCount = setMaxShowCount;
        }
        if (allDataCount > cells.Count) {
            for (var i = cells.Count; i < allDataCount; i++) {
                var rootTransform = this.transform as RectTransform;
                var cell = new UILoopCell(rootTransform, showWorldRect, hideRoot, cellDatas[i], updateItemFunc, i);
                cell.SetNeedSetChildWidth(NeedAutoSetChildWidth);
                cells.Add(cell);
            }
        }
        else if (allDataCount < cells.Count) { 
            var totalCellCount = cells.Count;
            for(var i= allDataCount;i< totalCellCount; i++) {
                var cellTemp = cells[cells.Count - 1];
                var go = cellTemp.OnHide();
                if (go != null) {
                    var cellType = cellTemp.GetCellType();
                    if (fressGos.TryGetValue(cellType, out List<GameObject> oFreeGos)) {
                        oFreeGos.Add(go);
                    }
                }
                cells.RemoveAt(cells.Count - 1);
            }
        }
        for (var i = 0; i < cells.Count; i++) {
            var cellTemp = cells[i];
            var go = cellTemp.OnHide();
            if (go != null) {
                var cellType = cellTemp.GetCellType();
                if (fressGos.TryGetValue(cellType, out List<GameObject> oFreeGos))
                {
                    oFreeGos.Add(go);
                }
            }
        }
        nextCellPos = startPos;
        nextOffCount = 0;
        nextCellType = -1;
        totalHeight = 0;
        float startY = 0;
        float endY = 0;
        for (var i = 0; i < cells.Count; i++)
        { 
            
        }
    }

    public int TestCount = 20;
#if UNITY_EDITOR
    [Button("�������")]
    private void Test()
    {
        var datas = new List<object>();
        for (var i = 0; i < TestCount; i++){
            datas.Add(i);
        }
        //SetData(datas, (oGo, oData) =>
        //{
        //    //Debug.LogError("oGo" + oGo.name);
        //    //Debug.LogError("oData" + oData.ToString());
        //});
    }
#endif
}
