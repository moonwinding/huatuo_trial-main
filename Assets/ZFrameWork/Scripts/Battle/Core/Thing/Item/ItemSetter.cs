using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creat/NewItem")]
public class ItemSetter:ScriptableObject
{
    public Item item;
    public Item GetItem(int pCount =1) {
        var newItem = new Item();
        if (item.ItemType == ItemType.HpItem)
        {
            newItem = new HpItem();
        }
        else if (item.ItemType == ItemType.MpItem)
        {
            newItem = new MpItem();
        }
        newItem.Type = item.Type;
        newItem.Id = item.Id;
        newItem.Count = pCount;
        newItem.Value = item.Value;
        newItem.ItemType = item.ItemType;
        newItem.Icon = item.Icon;
        newItem.Name = item.Name;
        return newItem;
    }
}
