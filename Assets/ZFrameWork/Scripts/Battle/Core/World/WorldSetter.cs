using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creat/NewWorld")]
public class WorldSetter : ScriptableObject
{
    public PlayerDataSetter playerDataSetter;
    public UnitSetter playerUnitSetter;
    public ThingsSetter thingsSetter;
    public List<UnitSetter> unitSetters;


    public WorldData GetWorldData() {
        var worldData = new WorldData();
        worldData.thingData = thingsSetter.GetThingData();

        worldData.playerData = playerDataSetter.GetPlayerData();
        int unitId = 1;
        worldData.playerUnit = playerUnitSetter.GetUnit(unitId);
        unitId++;
        worldData.units = new List<Unit>();
        for (var i = 0; i < unitSetters.Count; i++) {
            var unit = unitSetters[i].GetUnit(unitId);
            worldData.units.Add(unit);
            unitId++;
        }
        return worldData;
    }
}
public class WorldData{
    public PlayerData playerData;
    public Unit playerUnit;
    public ThingData thingData;
    public List<Unit> units;
}
