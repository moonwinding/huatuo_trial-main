using System.Collections;
using System.Collections.Generic;

public class EquipBag
{
    private List<Equip> equips = new List<Equip>();
    public List<Equip> GetEquips() {
        return equips;
    }
    public void AddEquip(Equip pEquip)
    {
        equips.Add(pEquip);
    }
    public void RemoveEquip(Equip pEquip)
    {
        equips.Remove(pEquip);
    }
}
