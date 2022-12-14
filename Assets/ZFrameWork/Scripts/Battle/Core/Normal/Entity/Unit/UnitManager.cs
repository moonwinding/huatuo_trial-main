using Battle.Skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager: IDriver
{
    private Dictionary<int, Unit> unitMap = new Dictionary<int, Unit>();
    private List<Unit> units = new List<Unit>();
    private int _id;
    private int selectUnitId;
    //private bool playerOwer;
    //private Vector3 clickPos;
    //private int targetUnitId;
    public void OnClickKeyCode(KeyCode pKeyCode,int pUId)
    {
        var uId = pUId > 0 ? pUId : selectUnitId;
        var unit = GetUnit(uId);
        if (unit != null && unit.PlayerOwer){
            unit.ReleaseSkill(1, () => { 

            });
        }
    }
    public void SelectTaragetId(int pTargetId){
        //targetUnitId = pTargetId;
        var unit = GetUnit(selectUnitId);
        if (unit != null && unit.PlayerOwer){
            var target = this.GetUnit(pTargetId);
            unit.AutoAttackTarget(target);
        }
    }
    public void OnSeletUnitId(int pUnitId, bool pPlayerOwer)
    {
        selectUnitId = pUnitId;
        //playerOwer = pPlayerOwer;
    }
    public void ClickPos(Vector3 pPos)
    {
        //clickPos = pPos;
        var unit = GetUnit(selectUnitId);
        if(unit != null && unit.PlayerOwer){
            unit.MoveToTargetPos(pPos,()=> {
                unit.Idle();
            });
        }
    }
    public int NewId() {_id++;return _id; }
    public void AddUnitByAttributeAndState(AttributeComponent pAttributeInfo,
        StateInfo pStateInfo,AIPolicyType pAIPolicyType, AIPath pAiPath){
        _id ++;
        var unit = new Unit(_id, pAttributeInfo, pStateInfo, AIAgentType.NavMeshAgent);
        AddUnit(unit);
    }
    public void RemoveUnit(int pUnitId) {
        var unit = GetUnit(pUnitId);
        if (unit != null){
            unit.OnDestory();
            unitMap.Remove(pUnitId);
            units.Remove(unit);
        }
    }
    public void AddUnit(Unit pUnit) {
        pUnit.OnInit();
        unitMap.Add(pUnit.Id, pUnit);
        units.Add(pUnit);
        pUnit.SetOnDeadCB(RemoveUnit);
    }
    public Unit GetUnit(int pId){
        Unit unit;
        unitMap.TryGetValue(pId,out unit);
        return unit;
    }
    public void OnInit() {
        EventG.BattleEnumEvent.AddListener(BattleEventType.Damg, OnBattleDmgEvent);
    }
    public void OnBattleDmgEvent(object pObj) {
        var dmgEvent = pObj as DamgeEvent;
        var unit = GetUnit(dmgEvent.owerId);
        if (unit != null) {
            var units_ = GetUnitsByDmgInfo(unit.campType, dmgEvent.owerId,unit.GetPos(),
                dmgEvent.targetType, dmgEvent.areaType, dmgEvent.range, unit.GetDir(), dmgEvent.angle);
            foreach (var temp in units_){
                temp.OnDmgEvent(unit, dmgEvent);
            }
        }
    }
    public List<Unit> GetUnitsSkillInfo(Skill pSkill,int pSkillOwerUId) {
        var unit = GetUnit(pSkillOwerUId);
        if (unit != null)
        {
            var units_ = GetUnitsByDmgInfo(unit.campType, pSkillOwerUId, unit.GetPos(),
                pSkill.targetType, pSkill.areaType, pSkill.range, unit.GetDir(), pSkill.angle);
            return units_;
        }
        return null;
    }
    public List<Unit> GetUnitsByDmgInfo(CampType pAtkCamp, int pAtkId,Vector3 pAtkPos,
        TargetSelectType pTargetType, AreaType pAreaType,float pRange, Vector3 pAtkDir,float pAngle) {
        var campUnits_ = GetUnitsByCampInfo(units, pAtkCamp, pAtkId, pTargetType,true);
        var areaUnits = GetUnitsByAreaInfo(campUnits_, pAreaType, pRange, pAtkPos, pAtkDir, pAngle);
        return areaUnits;
    }
    public List<Unit> GetUnitsByAreaInfo(List<Unit> pUnits, AreaType pAreaType, float pRange,Vector3 pAtkPos,Vector3 pAtkDir,float pAngle) {
        List<Unit> result = new List<Unit>();
        foreach (var temp in pUnits)
        {
            if (temp.IsInAreaRange(pAreaType, pRange, pAtkPos, pAtkDir, pAngle))
            {
                result.Add(temp);
            }
        }
        return result;
    }
    public List<Unit> GetUnitsByCampInfo(List<Unit> pUnits, CampType pAtkCamp,int pAtkId, 
        TargetSelectType pTargetType, bool pWithNpc = false)
    {
        List<Unit> result = new List<Unit>();
        foreach (var temp in pUnits)
        {
            if (temp.IsCanSelect(pAtkCamp, pAtkId, pTargetType, pWithNpc))
            {
                result.Add(temp);
            }
        }
        return result;
    }
    public void OnRun(float pTime, float pDeltaTime){
        foreach (var temp in units){
            temp.OnRun(pTime, pDeltaTime);
            temp.OnAiAction(pTime, pDeltaTime,units);
        }
    }
    public void OnDestory(){
        EventG.BattleEnumEvent.RemoveListener(BattleEventType.Damg, OnBattleDmgEvent);
        foreach (var temp in units){
            temp.OnDestory();
        }
        unitMap.Clear();
    }
}
