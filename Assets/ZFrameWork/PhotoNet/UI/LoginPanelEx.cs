using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanelEx : MonoBehaviour {
	public InputField AccountField;
	public InputField PassWordField;
	public Button LoginBtn;
	public Button RegBtn;
	public GameObject RegPanel;

	// Use this for initialization
	void Start () {
		LoginBtn.onClick.AddListener(()=> {
			Dictionary<byte, object> data = new Dictionary<byte, object>() {
				{ 1,AccountField.text},{ 2,PassWordField.text}
			};
			
			PhotonEngine.SendReq(Common.OperationCode.Login, data,(oData)=> { 
				
			});
		});
	}
}
