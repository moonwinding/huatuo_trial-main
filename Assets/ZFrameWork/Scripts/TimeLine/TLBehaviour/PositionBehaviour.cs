using UnityEngine;

public class PositionBehaviour : BaseBehaviour
{
    public Vector3 start;
    public Vector3 end;
    public bool useInitPos;
    public bool useWorldPos;
    public MoveType moveType = MoveType.P2P;//移动方式
    public Vector3 director;//方向
    public float distance;//距离
    private Vector3 dir;
    private Rigidbody2D rigiboDody2d;
    public MoveComponentType moveComponentType = MoveComponentType.Transform;
    protected override void OnStart()
    {
        if (moveType == MoveType.P2P)
        {
            if (!useInitPos)
            {
                if (timeLineInfer != null)
                {
                    if (useWorldPos)
                    {
                        start = target_.transform.position;
                        end = timeLineInfer.target.position;
                    }

                    else {
                        start = target_.transform.localPosition;
                        end = timeLineInfer.target.localPosition;
                    }
                        
                }
                else {
                    dir = end - start;
                    if (useWorldPos)
                        start = target_.transform.position;
                    else
                        start = target_.transform.localPosition;
                    end = start + dir;
                }
 
            }
        }
        else if(moveType == MoveType.Director)
        {
            if (useWorldPos)
                start = target_.transform.position;
            else
                start = target_.transform.localPosition;
            if(target_.transform.localScale.x > 0)
                end = start + target_.transform.right* distance;
            else
                end = start - target_.transform.right * distance;
            if(moveComponentType == MoveComponentType.Rigibody2d)
                rigiboDody2d = target_.GetComponent<Rigidbody2D>();
        }
        //Debug.LogError("On useInitPos >>" + useInitPos);
        //Debug.LogError("On Start >>"+ start);
        //Debug.LogError("On end >>" + end);
    }
    private Vector3 nextPos;
    private Vector3 curPos;
    protected override void OnProgress(float pProgress)
    {
        nextPos = start + (end - start) * pProgress;
        if (moveComponentType == MoveComponentType.Rigibody2d)
            rigiboDody2d.MovePosition(nextPos);
        else
        {
            if (useWorldPos)
                target_.transform.position = nextPos;
            else
                target_.transform.localPosition = nextPos;
        }
    }
    protected override void OnEnd()
    {
        if (moveComponentType == MoveComponentType.Rigibody2d && rigiboDody2d != null)
        {
            rigiboDody2d.MovePosition(rigiboDody2d.transform.position);
        }    
            
    }
}
