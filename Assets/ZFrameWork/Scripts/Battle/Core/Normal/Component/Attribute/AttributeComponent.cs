using System.Collections.Generic;

public class AttributeComponent
{
    private Dictionary<AttributeType, AttributeItem> attributes = 
        new Dictionary<AttributeType, AttributeItem>();

    public void AddAttributeChanges(List<AttributeChange> pChanges)
    {
        foreach (var temp in pChanges)
        {
            AddAttributeValue(temp.Type, temp.Value, temp.Max, temp.Recover);
        }
    }
    public void RemoveAttributeChanges(List<AttributeChange> pChanges) {
        foreach (var temp in pChanges)
        {
            AddAttributeValue(temp.Type, -temp.Value, -temp.Max, -temp.Recover);
        }
    }
    public void AddAttributeComponent(AttributeComponent pAttributeComponent) {
        foreach (var temp in pAttributeComponent.attributes) {
            AddAttributeValue(temp.Key,temp.Value.Value.Value, temp.Value.Value.Max, temp.Value.Value.Recover);
        }
    }
    public void RemoveAttributeComponent(AttributeComponent pAttributeComponent) {
        foreach (var temp in pAttributeComponent.attributes)
        {
            AddAttributeValue(temp.Key, -temp.Value.Value.Value, -temp.Value.Value.Max, -temp.Value.Value.Recover);
        }
    }
    public void AddAttributeItem(AttributeItem pAttributeItem){
        attributes.Add(pAttributeItem.Type, pAttributeItem);
    }
    public void AddAttribute(AttributeType pType,float pValue,float pMax = -1,float pRecover = 0){
        var attributeItem = new AttributeItem(pType, pValue, pMax, pRecover);
        AddAttributeItem(attributeItem);
    }
    public void AddAttributeValue(AttributeType pType,float pValue,float pMaxValue,float pRecover) {
        var attributeItem = GetAttributeItem(pType);
        if (attributeItem != null) {
            attributeItem.AddValue(pValue, pMaxValue, pRecover);
        }
    }
    public AttributeItem GetAttributeItem(AttributeType pType)
    {
        AttributeItem AttributeItem;
        attributes.TryGetValue(pType, out AttributeItem);
        return AttributeItem;
    }
    public void OnSecondRepeat() {
        foreach (var temp in attributes.Values)
        {
            temp.OnRecover();
        }
    }

}
