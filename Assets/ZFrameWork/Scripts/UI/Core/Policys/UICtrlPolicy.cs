using System.Collections.Generic;
using UnityEngine;

public class UICtrlPolicy: CtrlPolicyBase
{
	public UICtrlPolicy(GameObject pSubRoot, int pBaseLayer){
		SubRoot = pSubRoot;
		BaseLayer = pBaseLayer;
	}

	public override void Open(UICtrlBase pCtrl, System.Action pFinishCB, OpenData pOpenData)
	{
		Flow flow = new Flow("Open");
		flow.AddStep(1, (pOnFinish) => {
			pCtrl.DoInit();
			pOnFinish.Invoke( StepResult.Next,1);
		});
		flow.AddStep(2, (pOnFinish) => {
			if (pOpenData != null && pOpenData.ctrlData != null)
			{
				pCtrl.OnSetData(pOpenData.ctrlData);
			}
			pOnFinish.Invoke( StepResult.Next,1);
		});
		flow.AddStep(3, (pOnFinish) => {
			pCtrl.OnPost(pOnFinish);
		});
		flow.AddStep(4, (pOnFinish) => {
			pCtrl.ResLoad((oGO) => {
				if (oGO != null){
					pOnFinish.Invoke( StepResult.Next,1);
				}
				else{
					pOnFinish.Invoke(StepResult.Stop,1);
				}
			},SubRoot.transform);
		});
		//界面切前台
		flow.AddStep(5, (pOnFinish) => {
			pCtrl.DoForward();
			pOnFinish.Invoke(StepResult.Next,1);
		});
		//前面的顶层界面处理
		flow.AddStep(6, (pOnFinish) => {
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
			pOnFinish.Invoke( StepResult.Next,1);
		});
		flow.RunAllStep((pIsFinish) => {
			ctrls.Add(pCtrl);
			if(pFinishCB != null)
				pFinishCB.Invoke();
		});
	}
	public override void CloseTop(bool pNeedForward = true)
	{
		if (ctrls.Count > 0)
		{
			UICtrlBase ctrl = ctrls[ctrls.Count - 1];
			Flow flow = new Flow("CloseTop");
			flow.AddStep(1, (pOnFinish) => {
				ctrl.DoDispose();
				pOnFinish.Invoke(StepResult.Next,1);
			});
			flow.AddStep(2, (pOnFinish) => {
				ctrl.UnLoadRes();
				pOnFinish.Invoke(StepResult.Next,1);
			});
			flow.AddStep(3, (pOnFinish) => {
				if (ctrls.Count > 1)
				{
					var preCtrl = ctrls[ctrls.Count - 2];
					if (preCtrl != null && pNeedForward)
					{
						if (preCtrl.IsHaveUIBase())
						{
							preCtrl.DoForward();
							pOnFinish.Invoke(StepResult.Next,1);
						}
						else
						{
							preCtrl.ResLoad((oGO) =>
							{
								if (oGO != null)
								{
									preCtrl.DoForward();
									pOnFinish.Invoke(StepResult.Next,1);
								}
								else
								{
									pOnFinish.Invoke(StepResult.Stop,1);
								}
							}, SubRoot.transform);
						}
					}
					else
					{
						pOnFinish.Invoke(StepResult.Next,1);
					}
				}
				else
				{
					pOnFinish.Invoke(StepResult.Next,1);
				}
			});
			flow.RunAllStep((pIsFinish) => {
				ctrls.Remove(ctrl);
			});
		}
	}
}


