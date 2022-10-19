using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenFullInfo
{
	public UICtrlBase ctrl;
	public System.Action finishCB;
	public OpenData OpenData;
	public OpenFullInfo(UICtrlBase pUICtrlBase, System.Action pFinishCB, OpenData pOpenData)
	{
		ctrl = pUICtrlBase;
		finishCB = pFinishCB;
		OpenData = pOpenData;
	}
}
public class UICtrlPolicyQueue : CtrlPolicyBase
{
    public UICtrlPolicyQueue(GameObject pSubRoot, int pBaseLayer)
    {
        SubRoot = pSubRoot;
        BaseLayer = pBaseLayer;
    }

    private UICtrlBase curCtrlBase;
	private List<OpenFullInfo> Queue = new List<OpenFullInfo>();

	private void LoopOpenQueue()
	{
		if (Queue.Count > 0)
		{
			var OpenFullInfo = Queue[0];
			Queue.RemoveAt(0);
			Open(OpenFullInfo.ctrl, OpenFullInfo.finishCB, OpenFullInfo.OpenData);
		}
	}
	public override void CloseTop(bool pNeedForward = true)
	{
		if (curCtrlBase != null)
		{
			Flow flow = new Flow("CloseTop");
			flow.AddStep(1, (pOnFinish) => {
				curCtrlBase.DoDispose();
				pOnFinish.Invoke( StepResult.Next,1);
			});
			flow.AddStep(2, (pOnFinish) => {
				curCtrlBase.UnLoadRes();
				pOnFinish.Invoke(StepResult.Next,2);
			});
			flow.RunAllStep((pIsFinish) => {
				curCtrlBase = null;
				LoopOpenQueue();
			});
		}
		else
		{
			LoopOpenQueue();
		}
	}
	public override void Open(UICtrlBase pCtrl, System.Action pFinishCB, OpenData pOpenData)
	{
		if (curCtrlBase != null)
		{
			var openFullInfo = new OpenFullInfo(pCtrl, pFinishCB, pOpenData);
			Queue.Add(openFullInfo);
		}
		else
		{
			Flow flow = new Flow("Open");
			flow.AddStep(1, (pOnFinish) => {
				pCtrl.DoInit();
				pOnFinish.Invoke(StepResult.Next,1);
			});
			flow.AddStep(2, (pOnFinish) => {
				pCtrl.OnPost(pOnFinish);
			});
			flow.AddStep(3, (pOnFinish) => {
				pCtrl.ResLoad((oGO) => {
					if (oGO != null)
					{
						pOnFinish.Invoke(StepResult.Next, 2);
					}
					else
					{
						pOnFinish.Invoke(StepResult.Stop, 1);
					}
				}, SubRoot.transform);
			});
			//界面切前台
			flow.AddStep(4, (pOnFinish) => {
				pCtrl.DoForward();
				pOnFinish.Invoke(StepResult.Next,1);
			});
			//前面的顶层界面处理
			flow.AddStep(5, (pOnFinish) => {
				var topCtrl = GetTopCtrl();
				if (topCtrl != null && pOpenData != null)
				{
					if (pOpenData.OpenType == OpenType.DisablePre)
					{
						topCtrl.OnHide();
					}
					else if (pOpenData.OpenType == OpenType.DestoryPre)
					{
						topCtrl.UnLoadRes();
					}
				}
				pOnFinish.Invoke(StepResult.Next,1);
			});
			flow.RunAllStep((pIsFinish) => {
				pFinishCB.Invoke();
			});
			curCtrlBase = pCtrl;
		}
		
	}

}
