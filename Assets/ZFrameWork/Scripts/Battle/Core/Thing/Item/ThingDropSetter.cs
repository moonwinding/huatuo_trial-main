using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Creat/NewThingDrop")]
public class ThingDropSetter : ScriptableObject
{
    public List<ThingDrop> drops = new List<ThingDrop>();
    public AssetReference dropPrefab;
    public string GetDropPrefabPath() {
        return dropPrefab.AssetGUID;
    }
    public ThingGroup GetDropGroup()
    {
        var thingGroup = new ThingGroup();
        foreach (var temp in drops) {
            var count = temp.GetCount();
            if (count > 0) {
                var thing = BattleField.Instance.GetThing(temp.Type,temp.Id, count);
                thingGroup.AddThing(thing);
            }
        }
        return thingGroup;
    }
}