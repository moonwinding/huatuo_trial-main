using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creat/NewEquipAttribute")]
public class EquipAttributeSetter : ScriptableObject
{
    public List<AttributeChange> AttributeItems = new List<AttributeChange>();

    public List<AttributeChange> GetAttributeItems() {
        var items = new List<AttributeChange>();
        foreach (var item in AttributeItems) {
            var change = new AttributeChange();
            change.Type = item.Type;
            change.Value = item.Value;
            change.Max = item.Max;
            change.Recover = item.Recover;
            items.Add(change);
        }
        return items;
    }
}

[System.Serializable]
public class AttributeChange {
    public AttributeType Type;
    public float Value;
    public float Max;
    public float Recover;

}

