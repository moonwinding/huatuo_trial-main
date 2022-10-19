using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creat/NewItemBag")]
public class ItemBagSetter : ScriptableObject
{
    public List<ItemSetter> itemSetters = new List<ItemSetter>();
    public ItemBag GetItemBag()
    {
        var itemBag = new ItemBag();
        foreach (var temp in itemSetters) {
            var itemBase = temp.GetItem();
            itemBag.AddItem(itemBase);
        }
        return itemBag;
    }
}
