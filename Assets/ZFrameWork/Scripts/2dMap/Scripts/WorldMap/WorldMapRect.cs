using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapRect : MonoBehaviour
{
    public Rect rect;
    public GameObject resGo;
    public Transform bindParent;
    private GameObject go_;
    private bool _isContatin;
    public bool IsContains( Rect pRect) {
        _isContatin = rect.Overlaps(pRect);
        return _isContatin;
    }
    public void OnShow() {
        if (go_ != null){
            go_.gameObject.SetActive(true);
        }
        else {
            go_ = GameObject.Instantiate<GameObject>(resGo);
            go_.transform.parent = bindParent;
            go_.transform.localPosition = rect.center;
            go_.transform.localScale = Vector3.one;
        }
    }
    public void OnHide() {
        if (go_ != null)
        {
            go_.gameObject.SetActive(false);
        }
    }
    
}
