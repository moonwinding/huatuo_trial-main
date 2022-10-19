using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera25D : CameraBase
{
    public float clipX = 20;
    public float clipZ = 15;
    public float smoonthX = 1;
    public float smoonthZ = 1;
    public float smoonthScroll = 1;
    public Vector2 clampX;
    public Vector2 clampZ;
    public Vector2 clampScroll;
    private float screenWidth;
    private float screenHeight;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private Vector3 moveDir;
    private bool needMove = false;
    private Vector3 curCameraPos;
    private Vector3 curCameraRot;
    private bool isEnable = true;

    protected override void Awake()
    {
        base.Awake();
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        float centerX = screenWidth / 2;
        float centerY = screenHeight / 2;
        float xWidth = screenWidth / 2 - clipX;
        float yHeigth = screenHeight / 2 - clipZ;
        minX = centerX - xWidth;
        maxX = centerX + xWidth;
        minY = centerY - yHeigth;
        maxY = centerY + yHeigth;
    }
    protected override bool IsNeedUpdate()
    {
        return camera_ != null;
    }
    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isEnable = !isEnable;
        }
        if (!isEnable)
            return;
        moveDir = Vector3.zero;
        needMove = false;
        curCameraPos = camera_.transform.position;
        curCameraRot = camera_.transform.eulerAngles;
        if (Input.mousePosition.x <= minX && curCameraPos.x < clampX.y)
        {
            needMove = true;
            moveDir.x = -smoonthX * Time.deltaTime;
        }
        else if (Input.mousePosition.x >= maxX && curCameraPos.x > clampX.x)
        {
            needMove = true;
            moveDir.x = smoonthX * Time.deltaTime;
        }
        if (Input.mousePosition.y <= minY && curCameraPos.z < clampZ.y)
        {
            needMove = true;
            moveDir.z = -smoonthZ * Time.deltaTime;
        }
        else if (Input.mousePosition.y >= maxY && curCameraPos.z > clampZ.x)
        {
            needMove = true;
            moveDir.z = smoonthZ * Time.deltaTime;
        }
        var tempScroll = camera_.transform.eulerAngles.x;
        tempScroll += smoonthScroll * Input.mouseScrollDelta.y;
        tempScroll = Mathf.Clamp(tempScroll, clampScroll.x, clampScroll.y);
        camera_.transform.eulerAngles = new Vector3(tempScroll, 0, 0);
        if (needMove)
        {
            camera_.transform.position += moveDir;
        }
    }
}
