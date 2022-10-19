using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Equip:Thing
{
    public List<AttributeChange> changes;

    private Unit wearUnit;
    public void WearUnit(Unit pWearUnit){
        UnWear();
        wearUnit = pWearUnit;
        wearUnit.AddAttributeChanges(changes);
    }
    public void UnWear() {
        if (wearUnit != null) {
            wearUnit.RemoveAttributeChanges(changes);
            wearUnit = null;
        }
    }
    public bool IsWear() {
        return wearUnit != null;
    }
}
