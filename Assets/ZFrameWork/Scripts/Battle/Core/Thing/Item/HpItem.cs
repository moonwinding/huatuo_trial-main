public class HpItem : Item
{
    public override void OnUseSuccess(int pUseCount) {
        var unit = BattleField.Instance.GetPlayerUnit();
        unit.AddAttributeValue( AttributeType.Hp, Value* pUseCount,0,0);
    }
}
public class MpItem : Item
{
    public override void OnUseSuccess(int pUseCount)
    {
        var unit = BattleField.Instance.GetPlayerUnit();
        unit.AddAttributeValue(AttributeType.Mp, Value * pUseCount,0,0);
    }
}
