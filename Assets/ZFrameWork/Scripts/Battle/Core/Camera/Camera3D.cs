using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Camera3D : CameraBase
{
    
    private float _roll = -3f;
    public float scrollSpeed = 1f;
    public float rotSpeed = 1f;
    private Quaternion qCamera;
    private Quaternion qTarget;
    private float x;
    private float y;

    private float minY = -50f;
    private float maxY = 20f;
    private float moveXSpeed = 4f;
    private float moveYSpeed = 2f;
    private float rollMin = -10f;
    private float rollMax = -1f;
    private float height = 1f;

    protected override void Awake() {
        base.Awake();
    }

    protected override void OnUpdate() {
        if (Input.GetMouseButton(0))
        {
            x += Input.GetAxis("Mouse X") * moveXSpeed;
            y += Input.GetAxis("Mouse Y") * moveYSpeed;
            y = Mathf.Clamp(y, minY, maxY);
        }
        qCamera = Quaternion.Euler(-y, x, 0);
        qTarget = Quaternion.Euler(0, x, 0);
        cameraTransform.rotation = qCamera;
        if (Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X") * moveXSpeed;
            y += Input.GetAxis("Mouse Y") * moveYSpeed;
            target_.transform.localRotation = qTarget;
        }
        _roll += Input.GetAxis("Mouse ScrollWheel");
        _roll = Mathf.Clamp(_roll, rollMin, rollMax);
        cameraTransform.position = qCamera * new Vector3(0, height, _roll) + target_.transform.position;
    }
}
