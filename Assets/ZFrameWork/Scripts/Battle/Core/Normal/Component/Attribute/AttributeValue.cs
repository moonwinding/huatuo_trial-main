public class AttributeValue{
    public float Value;//当前的属性数值
    public float Max;//最大的属性数值 -1 为不限制上限
    public float Recover;//该数值的恢复数值(每秒恢复的数量)

    private System.Action<AttributeValue> onChange;
    public void AddChangeAction(System.Action<AttributeValue> pOnChangeAction) {
        onChange += pOnChangeAction;
    }
    public void RemoveChangeAction(System.Action<AttributeValue> pOnChangeAction) {
        onChange -= pOnChangeAction;
    }
    public AttributeValue(float pValue,float pMax = -1,float pRecover = 0) {
        Value = pValue;
        Max = pMax;
        Recover = pRecover;
    }
    //每秒调用的恢复
    public void OnRecover(){
        if(Recover != 0)
            AddValue(Recover);
    }
    //数值被外部改变
    public void AddValue(float pValue,float pAddMaxValue = 0f,float pRecover = 0f){
        Max += pAddMaxValue;
        Recover += pRecover;
        Value += pValue;
        if (Value < 0)
            Value = 0;
        if (Max != -1 && Value > Max)
            Value = Max;
        if (onChange != null)
        {
            onChange.Invoke(this);
        }
    }
    public float GetValue(){
        return Value;
    }
}
