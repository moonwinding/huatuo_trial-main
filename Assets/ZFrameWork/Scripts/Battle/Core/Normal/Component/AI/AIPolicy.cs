using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AIPolicyType
{ 
    None = 0,
    Soldier = 1,
    Tower = 2,
    Hero = 3,
    Monster = 4,
    Building = 5,
}
public enum AIAgentType { 
    None = 0,
    NavMeshAgent = 1,
    TitleSprite = 2,
}
public class BuildingPolicy : AIPolicy {
    protected override List<AIAction> allActions {
        get {
            if (allActions_ == null)
            {
                allActions_ = new List<AIAction>() {
                    new IdleAction()};
            }
            return allActions_;
        }
    }
    public BuildingPolicy(Unit pUnit)
    {
        bindAi = pUnit;
    }
}
public class HeroPolicy : AIPolicy {
    protected override List<AIAction> allActions {
        get {
            if (allActions_ == null)
            {
                allActions_ = new List<AIAction>() {
                    new BackToHomeAction(),
                    new SkillToTargetAction(),
                    new AttackToTargetAction(),
                    new MoveToTargetAction(),
                    new SelectTargetAction(),
                    new IdleAction()};
            }
            return allActions_;
        }
    }
    public HeroPolicy(Unit pUnit)
    {
        bindAi = pUnit;
    }
}
public class MonsterPolicy : AIPolicy {
    protected override List<AIAction> allActions {
        get {
            if (allActions_ == null)
            {
                allActions_ = new List<AIAction>() {
                    new BackToHomeAction(),
                    new AttackToTargetAction(),
                    new MoveToTargetAction(),
                    new SelectTargetAction(),
                    new IdleAction()};
            }
            return allActions_;
        }
    }
    public MonsterPolicy(Unit pUnit)
    {
        bindAi = pUnit;
    }
}
public class TowerPolicy : AIPolicy{
    protected override List<AIAction> allActions {
        get {
            if (allActions_ == null)
            {
                allActions_ = new List<AIAction>() {
                    new AttackToTargetAction(),
                    new SelectTargetAction(),
                    new IdleAction()};
            }
            return allActions_;
        }
    }
    public TowerPolicy(Unit pUnit)
    {
        bindAi = pUnit;
    }
}
public class SoldierPolicy : AIPolicy{
    protected override List<AIAction> allActions {
        get {
            if (allActions_ == null)
            {
                allActions_ = new List<AIAction>() {
                    new AttackToTargetAction(),
                    new MoveToTargetAction(),
                    new SelectTargetAction(),
                    new MoveAction(),
                    new IdleAction()};
            }
            return allActions_;
        }
    }
    public SoldierPolicy(Unit pMonsterAi) {
        bindAi = pMonsterAi;
    }
}
public class AIPolicy
{
    protected List<AIAction> allActions_;
    protected virtual List<AIAction> allActions { get { return allActions_;} } 
    private AIAction curAiAction;
    protected Unit bindAi;
    
    public AIPolicy() { }
    public AIPolicy(Unit pMonsterAi) {
        bindAi = pMonsterAi;
    }
    public void OnRun(List<Unit> pAllUnits)
    {
        for (var i = 0; i < allActions.Count; i++)
        {
            if (allActions[i].IsPass(bindAi, pAllUnits))
            {
                curAiAction = allActions[i];
                break;
            }
        }
        //Debug.LogError(curAiAction.GetType());
        curAiAction.OnAcion(bindAi, pAllUnits);
    }
}
//??????????????????????????
public class BackToHomeAction : AIAction{
    public override AIConditioner Conditioner { get { if (conditioner == null) conditioner = new BackToHomeConditioner(); return conditioner; } }
    public override void OnAcion(Unit pBindAi, List<Unit> pAllUnits){
        pBindAi.BackToHome();
    }
}
//????????????????????????????????
public class SkillToTargetAction : AIAction {
    public override AIConditioner Conditioner { get { if (conditioner == null) conditioner = new SkillToTargetConditioner(); return conditioner; } }
    public override void OnAcion(Unit pBindAi, List<Unit> pAllUnits)
    {
       
        pBindAi.ReleaseCurSkill();
    }
}
//??????????????????
public class AttackToTargetAction : AIAction
{
    public override AIConditioner Conditioner { get { if (conditioner == null) conditioner = new AttackToTargetConditioner(); return conditioner; } }
    public override void OnAcion(Unit pBindAi, List<Unit> pAllUnits)
    {
        pBindAi.StopMove();
        pBindAi.FaceToCurTarget();
        pBindAi.Attack();
    }
}
//????????????????
public class MoveToTargetAction : AIAction
{
    public override AIConditioner Conditioner { get { if (conditioner == null) conditioner = new MoveToTargetConditioner(); return conditioner; } }
    public override void OnAcion(Unit pBindAi, List<Unit> pAllUnits)
    {
        pBindAi.MoToCurTarget();
    }
}
//????????????????
public class SelectTargetAction : AIAction
{
    public override AIConditioner Conditioner { get { if (conditioner == null) conditioner = new ForcusTargetConditioner(); return conditioner; } }
    public override void OnAcion(Unit pBindAi, List<Unit> pAllUnits)
    {
        pBindAi.SelectAdjustTarget(pAllUnits);
    }
}
public class MoveAction : AIAction {
    public override AIConditioner Conditioner { get { if (conditioner == null) conditioner = new MoveConditioer(); return conditioner; } }
    public override void OnAcion(Unit pBindAi, List<Unit> pAllUnits)
    {
        pBindAi.OnLoopMove();
    }
}
public class IdleAction : AIAction {
    public override AIConditioner Conditioner { get {if (conditioner == null) conditioner = new IdleConditoner(); return conditioner; } }
    public override void OnAcion(Unit pBindAi, List<Unit> pAllUnits)
    {
        pBindAi.Idle();
    }
}
public class AIAction
{
    protected AIConditioner conditioner;

    public virtual AIConditioner Conditioner { get { if (conditioner == null) conditioner = new AIConditioner(); return conditioner; } }
    public virtual void OnAcion(Unit pBindAi, List<Unit> pAllUnits) { }
    public bool IsPass(Unit pBindAi, List<Unit> pAllUnits) {
        return Conditioner.IsPass(pBindAi, pAllUnits);
    }
}
//??????????????????????????
public class BackToHomeConditioner : AIConditioner
{
    public override bool IsPass(Unit pBindAi, List<Unit> pAllUnits)
    {
        return pBindAi.NeedBackToHome();
    }
}

//????????
public class SkillToTargetConditioner : AIConditioner
{
    public override bool IsPass(Unit pBindAi, List<Unit> pAllUnits)
    {
        return pBindAi.CanReleaserSkill();
    }
}
//????????
public class AttackToTargetConditioner : AIConditioner
{
    public override bool IsPass(Unit pBindAi, List<Unit> pAllUnits)
    {
        return pBindAi.CurTargetInRange(pBindAi.atkDistance);
    }
}
//????????
public class MoveToTargetConditioner : AIConditioner
{
    public override bool IsPass(Unit pBindAi, List<Unit> pAllUnits)
    {
        return pBindAi.CurTargetInRange(pBindAi.followDistance);
    }
}
//????????
public class ForcusTargetConditioner : AIConditioner
{
    public override bool IsPass(Unit pBindAi, List<Unit> pAllUnits)
    {
        return pBindAi.NeedSelectTarget(pAllUnits);
    }
}
//????d
public class MoveConditioer : AIConditioner {
    public override bool IsPass(Unit pBindAi, List<Unit> pAllUnits)
    {
        return !pBindAi.IsHaveTarget();
    }
}
//????
public class IdleConditoner: AIConditioner
{
    public override bool IsPass(Unit pBindAi, List<Unit> pAllUnits)
    {
        return !pBindAi.IsDead();
    }
}
public class AIConditioner
{
    public virtual bool IsPass(Unit pBindAi, List<Unit> pAllUnits)
    {
        return true;
    }
}