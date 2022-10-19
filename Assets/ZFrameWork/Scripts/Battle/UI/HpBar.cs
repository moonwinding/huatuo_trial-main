using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public float max;
    public Image hpForward;
    public Text hpText;
    public Image mpForward;
    public Text mpText;
    
    private float barHeight = 3f;
    private AttributeValue hpAttributeValue;
    private AttributeValue mpAttributeValue;
    private void Awake(){}
    public void SetHeight(float pHeight){
        barHeight = pHeight;
        this.transform.localPosition = Vector3.up * barHeight;
    }

    private void Start(){
        this.transform.localPosition = Vector3.up * barHeight;
    }

    public void BindHPValueItem(AttributeValue pValue) {
        if (hpAttributeValue != null)
        {
            hpAttributeValue.RemoveChangeAction(OnHPAttributeValueChange);
        }
        hpAttributeValue = pValue;
        pValue.AddChangeAction(OnHPAttributeValueChange);
    }
    public void BindMPValueItem(AttributeValue pValue)
    {
        if (mpAttributeValue != null)
        {
            mpAttributeValue.RemoveChangeAction(OnMPAttributeValueChange);
        }
        hpAttributeValue = pValue;
        pValue.AddChangeAction(OnMPAttributeValueChange);
    }

    private void OnHPAttributeValueChange(AttributeValue pValue) {
        SetHp(pValue.Value, pValue.Max);
    }
    private void OnMPAttributeValueChange(AttributeValue pValue)
    {
        SetMp(pValue.Value, pValue.Max);
    }

    public void SetMp(float pCurMp, float pMaxMp)
    {
        mpText.text = (int)pCurMp + "/" + pMaxMp;
        float progress = 1f;
        if (pMaxMp > 0)
            progress = pCurMp / (pMaxMp * 1f);
        var weight = progress * (max);
        mpForward.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, weight);// = new Vector3(progress * (max), 0, 0);
        mpForward.rectTransform.localPosition = new Vector3((weight - max) / 2, 0, 0);
    }
    public void SetHp(float pCurHp,float pMaxHp)
    {
        hpText.text = (int)pCurHp + "/" + pMaxHp;
        var progress = pCurHp / (pMaxHp * 1f);
        var weight = progress * (max);
        hpForward.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, weight);// = new Vector3(progress * (max), 0, 0);
        hpForward.rectTransform.localPosition = new Vector3((weight - max) / 2, 0, 0);
    }
    private void OnDestroy()
    {
        if (hpAttributeValue != null)
        {
            hpAttributeValue.RemoveChangeAction(OnHPAttributeValueChange);
        }
        if (mpAttributeValue != null)
        {
            mpAttributeValue.RemoveChangeAction(OnMPAttributeValueChange);
        }
    }
    //private void LateUpdate()
    //{
    //    this.transform.rotation = Camera.main.transform.rotation;
    //    this.transform.LookAt(Camera.main.transform);
    //}
    //private void FixedUpdate()
    //{
    //    this.transform.rotation = Camera.main.transform.rotation;
    //    this.transform.LookAt(Camera.main.transform);
    //}
    private void Update()
    {
        this.transform.rotation = Camera.main.transform.rotation;
        //this.transform.LookAt(Camera.main.transform);
    }
}
