using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creat/TingsSetter")]
public class ThingsSetter : ScriptableObject
{
    public List<ItemSetter> Items = new List<ItemSetter>();
    public List<EquipSetter> Equips = new List<EquipSetter>();
    
    public ThingData GetThingData()
    {
        var thingData = new ThingData();
        foreach (var temp in Items)
        {
            thingData.ItemMap.Add(temp.item.Id, temp);
        }
        foreach (var temp in Equips)
        {
            thingData.EquipMap.Add(temp.equip.Id, temp);
        }
        return thingData;
    }
}
public class ThingData {
    public Dictionary<int, ItemSetter> ItemMap = new Dictionary<int, ItemSetter>();
    public Dictionary<int, EquipSetter> EquipMap = new Dictionary<int, EquipSetter>();
    public Thing GetThing(ThingType pThingType, int pId, int pCount)
    {
        if (pThingType == ThingType.Item)
            return GetItem(pId, pCount);
        else if (pThingType == ThingType.Equip)
            return GetEquip(pId);
        return null;
    }
    public Equip GetEquip(int pId)
    {
        var equipBase_ = EquipMap[pId];
        var newEquip = equipBase_.GetEquip();
        return newEquip;
    }
    public Item GetItem(int pId, int pCount)
    {
        var itemBase_ = ItemMap[pId];
        var newItem = itemBase_.GetItem(pCount);
        return newItem;
    }

}
