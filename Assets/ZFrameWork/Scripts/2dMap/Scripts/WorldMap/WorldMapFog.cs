using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldMapFog : MonoBehaviour
{
    public TileBase fullFogTitle;
    public TileBase hafFogTitle;
    public TileBase noFogTitle;
    
    public Transform target;
    public int viewSize;
    private int checkSize;
    private Vector3 curPos;
    private Vector3Int curPosInt;
    private Vector3Int nextPosInt;
    private List<Vector3Int> ViewPosList = new List<Vector3Int>();
    
    private Grid m_Grid;
    private Tilemap m_Titlemap;
    public CameraSmoothFollow cameraSmoothFollow;
    private void Awake()
    {
        checkSize = viewSize + 1;
    }
    void Start()
    {
        m_Grid = GetComponent<Grid>();
        m_Titlemap = GetComponentInChildren<Tilemap>();

    }
    private void UpdateViewFog() {
        if (target != null)
        {
            curPos = target.transform.position;
            nextPosInt = m_Grid.WorldToCell(curPos);
            if (nextPosInt == curPosInt)
                return;
            curPosInt = nextPosInt;
            for (int i = -checkSize; i <= checkSize; i++)
            {
                for (int j = -checkSize; j <= checkSize; j++)
                {
                    var pos = curPosInt + new Vector3Int(i, j, 0);
                    if ((Mathf.Abs(i) + Mathf.Abs(j)) <= viewSize){
                        //ViewInfoMap.Add(pos,true);
                        OnView(pos,true);
                        if(!ViewPosList.Contains(pos))
                            ViewPosList.Add(pos);
                    }
                    else {
                        //ViewInfoMap.Add(pos, false);
                        OnView(pos, false);
                    }
                }
            }
        }
        else {
            target = cameraSmoothFollow.target;
        }
    }
    private void OnView(Vector3Int pPos,bool pInView) {

        TileBase titleBase = null;
        if (pInView)
            titleBase = noFogTitle;
        else
        {
            if (ViewPosList.Contains(pPos))
            {
                titleBase = hafFogTitle;
            }
            else
            {
                titleBase = fullFogTitle;
            }
        }
        if (m_Titlemap.GetTile(pPos) != titleBase)
            m_Titlemap.SetTile(pPos, titleBase);
    }
    void Update()
    {
        UpdateViewFog();
    }
}
