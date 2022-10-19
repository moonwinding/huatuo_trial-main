using System.Collections.Generic;

public class PlayerData
{
    public ItemBag itemBag;
    public EquipBag equipBag;
    public List<Thing> GetThings() {
        var things = new List<Thing>();
        var itemThings = itemBag.GetThings();
        things.AddRange(itemThings);
        var equipThings = equipBag.GetEquips();
        things.AddRange(equipThings);
        return things;
    }
    public void AddThingGroup(ThingGroup pThingGroup) {
        foreach (var temp in pThingGroup.Things) {
            AddThing(temp);
        }
    }
    public void AddThing(Thing pThing) {
        if (pThing.Type == ThingType.Item)
        {
            itemBag.AddThing(pThing);
        }
        else if (pThing.Type == ThingType.Equip) {
            equipBag.AddEquip(pThing as Equip);
        }
    }
    public void OnUseItem(int pItemId,int pCount) {
        itemBag.UseThing(pItemId, pCount);
    }
}
