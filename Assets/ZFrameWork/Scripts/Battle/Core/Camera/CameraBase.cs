using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBase : MonoBehaviour
{
    protected Camera camera_;
    protected GameObject target_;
    protected Transform cameraTransform;

    protected virtual void Awake()
    {
        if (camera_ == null)
            camera_ = Camera.main;
        if (camera_ != null)
            cameraTransform = camera_.transform;
    }
    

    public void SetTarget(GameObject pTarget)
    {
        target_ = pTarget;
    }
    private void Update()
    {
        if (!IsNeedUpdate()) {
            return;
        }
        OnUpdate();
    }
    protected virtual bool IsNeedUpdate() {
        if (target_ == null || cameraTransform == null)
            return false;
        return true;
    }
    protected virtual void OnUpdate()
    {
        
    }
}
