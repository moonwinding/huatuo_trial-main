using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleModel
{ 
    Normal =  1,    //普通模式
    ECS = 2,        //Ecs 战场框架
}

public class BattleField : MonoBehaviour
{
    public BattleModel model = BattleModel.Normal;
    public GameObject Ecs;
    public NormalBattle Normal;
    public GameObject GlobalRoot;
    public WorldSetter worldSetter;

    private WorldData worldData;
    private ThingData thingData;
    private Unit playerUnit;
    private PlayerData playerData;

    private static BattleField instance;
    public static BattleField Instance { get { return instance; } }
    public PlayerData GetPlayerData() { return playerData;}

    public Item GetItem(int pId,int pCount) {
        return thingData.GetItem(pId, pCount);
    }
    
    public Thing GetThing(ThingType pThingType,int pId, int pCount)
    {
        return thingData.GetThing(pThingType,pId, pCount);
    }

    private void Awake(){
        instance = this;
       
        worldData = worldSetter.GetWorldData();
        thingData = worldData.thingData;
        playerData = worldData.playerData;
        
    }
    void Start(){
        if (model == BattleModel.ECS){
            Ecs.SetActive(true);
        }
        else{
            Normal.gameObject.SetActive(true);
        }
    }
    public Unit GetPlayerUnit() {return playerUnit;}
    public void SetPlayerUnit(Unit pUnit){playerUnit = pUnit;OnPlayerLoad();}
    public void OnPlayerLoad(){
       // UICtrlManager.OpenBaseUI(new LoginPanelCtrl(), () => { });
    }
    public UnitManager GetUnitManager(){return Normal.GetUnitManager();}
    public void OnClickKeyCode(KeyCode pKeyCode,int pUId) {Normal.OnClickKeyCode(pKeyCode, pUId);}
    public int AddDelayAction(float pDelayTime, System.Action pOnAction){return Normal.AddDelayAction(pDelayTime, pOnAction);}
    public void RemoveDelayAction(int pAcId){Normal.RemoveDelayAction(pAcId);}
    public Unit GetUnit(int pUId) {return Normal.GetUnit(pUId);}
    public void SelectTaragetId(int pTargetId) {Normal.SelectTaragetId(pTargetId);}
    public void OnSeletUnitId(int pUnitId,bool pPlayerOwer) {Normal.OnSeletUnitId(pUnitId, pPlayerOwer);}
    public void ClickPos(Vector3 pPos) {Normal.ClickPos(pPos);}
}
