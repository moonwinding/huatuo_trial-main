using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creat/NewPlayerData")]
public class PlayerDataSetter : ScriptableObject
{
    public ItemBagSetter itemBagSetter;
    public EquipBagSetter equipSetter;
    public PlayerData GetPlayerData()
    {
        var playerData = new PlayerData();
        playerData.itemBag = itemBagSetter.GetItemBag();
        playerData.equipBag = equipSetter.GetEquipBag();
        return playerData;
    }
}
