using UnityEngine;
using UnityEngine.Playables;

public enum MoveType { 
	P2P = 0,//通过两点之间移动
	Director = 1,//通过初始点和方向加距离移动
}
public enum MoveComponentType { 
	Transform = 0,
	Rigibody2d = 1,
}
[System.Serializable]
public class PositionAsset : BaseAsset
{
	public Vector3 start;
	public Vector3 end;
	public MoveType moveType = MoveType.P2P;//移动方式
	public MoveComponentType moveComponentType = MoveComponentType.Transform;
	public Vector3 director;//方向
	public float distance;//距离
	public bool UseInitPos = true;
	public bool UseWorldPos = false;
	public override BaseBehaviour GetBehaviour(PlayableGraph graph, GameObject go)
	{
		var baseBehaviour = new PositionBehaviour();
		baseBehaviour.target_ = target_.GetGo(graph, go, out baseBehaviour.parent_);
		var timeInfoer = go.GetComponent<TimeLineInfoer>();
		baseBehaviour.timeLineInfer = timeInfoer;
		baseBehaviour.start = start;
		baseBehaviour.end = end;
		baseBehaviour.useInitPos = UseInitPos;
		baseBehaviour.useWorldPos = UseWorldPos;
		baseBehaviour.moveType = moveType;
		baseBehaviour.director = director;
		baseBehaviour.distance = distance;
		baseBehaviour.moveComponentType = moveComponentType;

		return baseBehaviour;
	}
}
