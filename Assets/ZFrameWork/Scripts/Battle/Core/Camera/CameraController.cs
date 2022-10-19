using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraControllerType 
{ 
    Camera2d,
    Camera25d,
    Camera3d,
}

public class CameraController : MonoBehaviour
{
    public CameraControllerType type = CameraControllerType.Camera3d;
    
    private static CameraController instance;
    public static CameraController Instance { get { return instance; } }

    private CameraBase cameraBase;
    private void Awake()
    {
        instance = this;
        if (type == CameraControllerType.Camera3d)
        {
            cameraBase = this.gameObject.AddComponent<Camera3D>();
        }
        else if (type == CameraControllerType.Camera2d)
        {
            cameraBase = this.gameObject.AddComponent<Camera2D>();
        }
        else if (type == CameraControllerType.Camera25d) {
            cameraBase = this.gameObject.AddComponent<Camera25D>();
        }
    }
    public void SetTarget(GameObject pTarget) {
        if (cameraBase != null)
            cameraBase.SetTarget(pTarget);
    }
}
