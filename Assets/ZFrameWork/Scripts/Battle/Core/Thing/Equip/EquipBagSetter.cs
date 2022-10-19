using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creat/NewEquipBag")]
public class EquipBagSetter : ScriptableObject
{
    public List<EquipSetter> equipSetters = new List<EquipSetter>();

    public EquipBag GetEquipBag()
    {
        var equipBag = new EquipBag();
        foreach (var temp in equipSetters)
        {
            var equip = temp.GetEquip();
            equipBag.AddEquip(equip as Equip);
        }
        return equipBag;
    }

}
