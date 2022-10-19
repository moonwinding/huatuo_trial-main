using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APanelCtrl : UICtrlBase
{
    protected override string Key { get { return "APanelCtrl"; } }
    protected override string Path { get { return "Assets/_ABs/LocalDontChange/UIPrefabs/Test/APanel.prefab"; } }

    protected override void OnInit()
    {
		var fullData1 = new ComponentSetterFullData();
		fullData1.AddComponentData(1,new ButtonData("",()=>{
		var ctrl = new APanelCtrl();
		UICtrlManager.OpenBaseUI(ctrl,null);
	}));
		fullData1.AddComponentData(2,new TextData("_Txt_","打开界面B"));
		fullData.AddComponentData(1,new ComponentSetterData("_Btn_OpenUI_APanel",fullData1));
		fullData.AddComponentData(2,new TextData("_Txt_Trans_1","dadad"));
		fullData.AddComponentData(3,new TextData("_Txt_Trans_1/_Txt_Trans_1","dadad"));
		var fullData2 = new ComponentSetterFullData();
		fullData2.AddComponentData(1,new ButtonData("",() =>{ UICtrlManager.CloseTopBaseUI(); }));
		fullData2.AddComponentData(2,new TextData("_Txt_","返回"));
		fullData.AddComponentData(4,new ComponentSetterData("_Back_Btn_",fullData2));
		var fullData3 = new ComponentSetterFullData();
		fullData3.AddComponentData(1,new ButtonData("",()  =>{ UICtrlManager.RevokeToHomeBaseUI(); }));
		fullData3.AddComponentData(2,new TextData("_Txt_","主页"));
		fullData.AddComponentData(5,new ComponentSetterData("_Home_Btn_",fullData3));
		var count = 1;
		fullData.AddComponentData(6,new TypeMakerData("_TypeMaker",count,(oGO,oIndex)=>{
			///ComponentSetter
			var comSetter = oGO.GetComponent<ComponentSetter>();
			///ComponentSetterFullData
			var fullDataTemp = new ComponentSetterFullData();
			fullDataTemp.AddComponentData(1,new ImageData("_Img_",null));
			fullDataTemp.AddComponentData(2,new TextData("_Txt_","测试"));
			comSetter.SetComponentDataByFullData(fullDataTemp);
		}));
    }
	protected override void OnForward() {
		SetFullCommonentData(fullData);
	}
}

