using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPanelCtrl : UICtrlBase
{
    protected override string Key { get { return "BPanelCtrl"; } }
    protected override string Path { get { return "Assets/_ABs/LocalDontChange/UIPrefabs/Test/BPanel.prefab"; } }

    protected override void OnInit()
    {
		var fullData1 = new ComponentSetterFullData();
		fullData1.AddComponentData(1,new ButtonData("",()=> {}));
		fullData1.AddComponentData(2,new TextData("_Txt_1","打开界面B"));
		fullData.AddComponentData(1,new ComponentSetterData("_BtnB_Pos_Btn_",fullData1));
		fullData.AddComponentData(2,new TextData("_Txt_Trs_scale_","标题1"));
		fullData.AddComponentData(3,new TransformData("_Txt_Trs_scale_",Vector3.one,"scale"));
		fullData.AddComponentData(4,new TextData("_Txt_Trs_scale_/_Txt_Trs_2","标题"));
		fullData.AddComponentData(5,new TransformData("_Txt_Trs_scale_/_Txt_Trs_2",Vector3.zero));
		var fullData2 = new ComponentSetterFullData();
		fullData2.AddComponentData(1,new ButtonData("",()=>{ UICtrlManager.CloseTopBaseUI(); }));
		fullData2.AddComponentData(2,new TextData("_Txt_1","返回"));
		fullData.AddComponentData(6,new ComponentSetterData("_Back_Btn_",fullData2));
		var fullData3 = new ComponentSetterFullData();
		fullData3.AddComponentData(1,new ButtonData("",()=>{ UICtrlManager.RevokeToHomeBaseUI(); }));
		fullData3.AddComponentData(2,new TextData("_Txt_1","主页"));
		fullData.AddComponentData(7,new ComponentSetterData("_Home_Btn_",fullData3));
		var fullData4 = new ComponentSetterFullData();
		fullData4.AddComponentData(1,new ButtonData("",()=> {}));
		fullData4.AddComponentData(2,new TextData("_Txt_1","打开界面C"));
		fullData.AddComponentData(8,new ComponentSetterData("_BtnC_Pos_Btn_",fullData4));
		var count = 1;
		fullData.AddComponentData(9,new TypeMakerData("_TypeMaker",count,(oGO,oIndex)=>{
			///ComponentSetter
			var comSetter = oGO.GetComponent<ComponentSetter>();
			///ComponentSetterFullData
			var fullDataTemp = new ComponentSetterFullData();
			fullDataTemp.AddComponentData(1,new ImageData("_Img_",null));
			fullDataTemp.AddComponentData(2,new TextData("_Txt_","测试"));
			comSetter.SetComponentDataByFullData(fullDataTemp);
		}));
    }
	protected override void OnForward()
	{
		SetFullCommonentData(fullData);
	}
}

