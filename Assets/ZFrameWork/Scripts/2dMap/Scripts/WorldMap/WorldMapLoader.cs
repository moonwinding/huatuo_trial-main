using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapLoader : MonoBehaviour
{
    public Transform target;
    public int offet;
    public int cellSize = 60;
    private Rect rect;
    private WorldMapRect[] rects;
    // Start is called before the first frame update
    void Start(){
        rects = this.gameObject.GetComponentsInChildren<WorldMapRect>();
        rect.width = Screen.width/ cellSize + offet;
        rect.height = Screen.height/ cellSize + offet;
    }
    
    // Update is called once per frame
    void Update(){
        rect.center = target.position;
        foreach (var temp in rects) {
            if (temp.IsContains(rect)){
                temp.OnShow();
            }
            else {
                temp.OnHide();
            }
        }
    }
}
