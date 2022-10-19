using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBag
{
    private Dictionary<int, Thing> itemMap = new Dictionary<int, Thing>();
    public List<Thing> GetThings() {
        var items = new List<Thing>();
        foreach (var temp in itemMap) {
            if(temp.Value.Count > 0)
                items.Add(temp.Value);
        }
        return items;
    }
    
    public void AddThing(Thing pThing) {
        if (!itemMap.ContainsKey(pThing.Id))
        {
            itemMap.Add(pThing.Id, pThing);
        }
        else
        {
            itemMap[pThing.Id].Count += pThing.Count;
        }
    }
    public void AddItem(Item pItem) {
        if (!itemMap.ContainsKey(pItem.Id)){
            itemMap.Add(pItem.Id, pItem);
        }
        else {
            itemMap[pItem.Id].Count += pItem.Count;
        }
    }
    public void RemoveItem(Item pItem) {
        if (itemMap.ContainsKey(pItem.Id)){
            itemMap[pItem.Id].Count -= pItem.Count;
            if (itemMap[pItem.Id].Count < 0)
                itemMap[pItem.Id].Count = 0;
        }
    }
    public void UseThing(int pId,int pCount) {
        var thing = GetThing(pId);
        if (thing != null) {
            thing.OnUse(pCount);
        }
    }
    public Thing GetThing(int pId) {
        if (itemMap.ContainsKey(pId)) {
            return itemMap[pId];
        }
        return null;
    }
}
