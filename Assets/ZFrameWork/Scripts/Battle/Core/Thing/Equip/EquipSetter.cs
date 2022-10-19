using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creat/NewEquip")]
public class EquipSetter: ScriptableObject
{
    public Equip equip;

    public Equip GetEquip()
    {
        var equipBase = new Equip();
        equipBase.Id = equip.Id;
        equipBase.Icon = equip.Icon;
        equipBase.Name = equip.Name;
        equipBase.changes = equip.changes;
        equipBase.Count = 1;
        equipBase.Type = equip.Type;
        return equipBase;
    }
}
