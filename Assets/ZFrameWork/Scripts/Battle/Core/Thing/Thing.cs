using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThingType { 
    Item = 0,
    Equip = 1,
}

[System.Serializable]
public class ThingDrop
{
    public int Id;
    public int Count;
    public int Weight;// 0 - 100 掉落的数量 从0 到最大
    public ThingType Type = ThingType.Item;//物品类型
    public int GetCount(){
        var random = UnityEngine.Random.Range(0, 100);
        return (int)(Count * random / 100f);
    }
}
public class ThingGroup {
    public List<Thing> Things = new List<Thing>();
    public void AddThing(Thing pThing) {
        Things.Add(pThing);
    }
}
[System.Serializable]
public class Thing
{
    public Sprite Icon;
    public int Id;
    public int Count;
    public string Name;
    public ThingType Type = ThingType.Item;
    public virtual bool OnUse(int pCount) {
        return false;
    }
}
