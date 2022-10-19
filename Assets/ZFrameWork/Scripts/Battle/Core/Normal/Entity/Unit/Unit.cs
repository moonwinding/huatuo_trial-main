using System.Collections.Generic;
using UnityEngine;
using Battle.Skill;
using UnityEngine.AI;
public class Unit:IDriver
{
    public int Id;
    public bool PlayerOwer;
    public float followDistance { get { return AttributeInfo.GetAttributeItem(AttributeType.FlowRange).Value.Value; } }
    public float atkDistance { get { return AttributeInfo.GetAttributeItem(AttributeType.AtkRange).Value.Value; } }
    public float mp { get { return AttributeInfo.GetAttributeItem(AttributeType.Mp).Value.Value; } }
    public CampType campType { get { return StateInfo.campType; } }
    private float atkCd { get { return AttributeInfo.GetAttributeItem(AttributeType.AtkCd).Value.Value; } }
    private float atk { get { return AttributeInfo.GetAttributeItem(AttributeType.Atk).Value.Value; } }
    private float warnDistance { get { return AttributeInfo.GetAttributeItem(AttributeType.WarnRange).Value.Value; } }
    
    private bool IsNeedBackToHome = false;
    private Vector3 findTargetPos;
    private Vector3 targetPos;
    private float nextAtkTime;
    private float nextTime = 0;
    private Unit Target;
    private AttributeComponent AttributeInfo;
    private StateInfo StateInfo;
    private SkillInfo skillInfo;
    private AIPath path;
    private SkillReleaser skillReleaser;
    private GameObject model;
    private AIAgentType agentType;
    private HpBar hpBar;
    private GameObject unit;
    private UnitAgent unitAgent;
    private UnitController unitController;
    private UnitAnimation unitAni;
    //private SpriteAnimation spriteAnimation;
    private AIPolicy aiPolicy;
    private AIPolicyType aiPolicyType;
    private bool isDead = false;
    private System.Action<int> onDead;
    private ThingDropSetter itemDropSetter;
    public void SetItemDropSetter(ThingDropSetter pItemDropSetter) {
        itemDropSetter = pItemDropSetter;
    }
    public void AddAttributeChanges(List<AttributeChange> pChanges) {
        AttributeInfo.AddAttributeChanges(pChanges);
    }
    public void RemoveAttributeChanges(List<AttributeChange> pChanges)
    {
        AttributeInfo.RemoveAttributeChanges(pChanges);
    }
    public void AddAttributeComponent(AttributeComponent pAttributeComponent)
    {
        AttributeInfo.AddAttributeComponent(pAttributeComponent);
    }
    public void RemoveAttributeComponent(AttributeComponent pAttributeComponent)
    {
        AttributeInfo.RemoveAttributeComponent(pAttributeComponent);
    }
    public void AddAttributeValue(AttributeType pType, float pValue,float pMaxValue, float pRecover)
    {
        AttributeInfo.AddAttributeValue(pType, pValue, pMaxValue, pRecover);
    }
    public void OnCostMp(float pCost) {
        var mpValue = AttributeInfo.GetAttributeItem(AttributeType.Mp);
        mpValue.Value.AddValue(-pCost);
    }
    public bool IsInAreaRange(AreaType pAreaType, float pRange,Vector3 pAtkPos,Vector3 pAtkDir,float pAngle) {
        var pos = this.unit.transform.position;
        if (pAreaType == AreaType.Sphere)
        {
            return Vector3.Distance(pos, pAtkPos) <= pRange;
        }
        else if (pAreaType == AreaType.ForwardAngle)
        {
            if (Vector3.Distance(pos, pAtkPos) <= pRange)
            {
                var tempPos = new Vector3(pos.x,0, pos.z);
                var angle = Vector3.Angle(pAtkDir, (tempPos - pAtkPos).normalized);
                return (Mathf.Abs(angle)) <= pAngle;
            }
        }
        return false;
    }
    public bool IsCanSelect(CampType pAtkCamp,int pAtkUId,TargetSelectType pSelectType,bool pWithNpc = false) {
        if (!IsDead()) {
            if (pSelectType == TargetSelectType.Self)
            {
                return pAtkUId == this.Id;
            }
            else if (pSelectType == TargetSelectType.Friend) {
                return pAtkCamp == this.campType;
            }
            else if (pSelectType == TargetSelectType.Enemy) {
                if(pWithNpc)
                    return pAtkCamp != this.campType;
                else
                    return pAtkCamp != this.campType && this.campType != CampType.Npc;
            }
        }
        return false;
    }
    public Unit(int pId, AttributeComponent pAttributeInfo, StateInfo pStateInfo, AIAgentType pAgentType) {
        Id = pId;
        AttributeInfo = pAttributeInfo;
        StateInfo = pStateInfo;
        agentType = pAgentType;
    }
    public void SetBornPos(Vector3 pBornPos) {
        StateInfo.bornPos = pBornPos;
    }
    public void SetOnDeadCB(System.Action<int> pOnDead) {onDead = pOnDead;}
    public void SetPlayerOwer(bool pPlayerOwer) {
        this.PlayerOwer = pPlayerOwer;
        //Debug.LogError($"playerower {this.PlayerOwer} id is {this.Id}");
    }
    public void LoadAiPath(AIPath pPath) {path = pPath;}
    public void SetSkillInfo(SkillInfo pSkillInfo) {
        skillInfo = pSkillInfo;
    }
    public void ReleaseSkill(int pSkillId,System.Action pFinish) {
        var skill = skillInfo.GetSkill(pSkillId);
        if (skillReleaser.IsCanRelease(skill)){

            if(agentType == AIAgentType.NavMeshAgent)
                unitAgent.SetNavMeshAgentEnable(false);
            if (unitController != null)
                unitController.enabled = false;

            skillReleaser.ReleaseSkill(skill,Target.model,() => {
                if (agentType == AIAgentType.NavMeshAgent)
                    unitAgent.SetNavMeshAgentEnable(true);
                if (unitController != null && this.PlayerOwer)
                    unitController.enabled = true;
                pFinish?.Invoke();
            });
        }
    }
    public void LoadAIPolicy(AIPolicyType pAiType) {
        aiPolicyType = pAiType;
        if (pAiType == AIPolicyType.Soldier)
        {
            aiPolicy = new SoldierPolicy(this);
        }
        else if (pAiType == AIPolicyType.Tower)
        {
            aiPolicy = new TowerPolicy(this);
        }
        else if (pAiType == AIPolicyType.Monster)
        {
            aiPolicy = new MonsterPolicy(this);
        }
        else if (pAiType == AIPolicyType.Hero)
        {
            aiPolicy = new HeroPolicy(this);
        }
        else if (pAiType == AIPolicyType.Building)
        {
            aiPolicy = new BuildingPolicy(this);
        }
    }
    public bool IsDead(){return isDead;}
    public void OnAiAction(float pTime, float pDeltaTime,List<Unit> pAllUnits) {
        //Debug.LogError($"OnAiAction {this.PlayerOwer} id is {this.Id}");
        if (PlayerOwer)
            return;
        if (aiPolicy != null && !IsDead()){
            aiPolicy.OnRun(pAllUnits);
        }
    }
    public void OnRun(float pTime, float pDeltaTime){
        if (pTime >= nextTime){
            nextTime += 1;
            OnSecondRepeat();
        }
    }
    public void AutoAttackTarget(Unit pTarget)
    {
        var targetUnit = pTarget;
        SetCurTarget(pTarget);
        if (targetUnit != null && !targetUnit.IsDead())
        {
            if (targetUnit.campType != campType)
            {
                if (CurTargetInRange(atkDistance))
                {
                    SelfAtk(() =>
                    {
                        AutoAttackTarget(Target);
                    });
                }
                else
                {
                    MoveToTargetPos(targetUnit.GetPos(), () =>
                    {
                        SelfAtk(() =>
                        {
                            AutoAttackTarget(Target);
                        });
                    });
                }
            }
            else
            {
                MoveToTargetPos(targetUnit.GetPos(), () =>
                {
                    Idle();
                });
            }
        }
        else {

            Idle();
        }
    }
    private void PlayAni(AniNameType pAniType, int pProx, bool pIsForce = true, System.Action pOnFinish = null) {
        unitAni.PlayAniByType(pAniType, pProx, pIsForce, pOnFinish);
    }
    public void OnInit() {
        if(unit == null){
            unit = new GameObject("Unit"+Id);
            unit.transform.SetParent(BattleField.Instance.GlobalRoot.transform);
            unit.transform.SetPositionAndRotation(StateInfo.GetPos(), Quaternion.identity);
            if (agentType == AIAgentType.NavMeshAgent)
                unitAgent = unit.AddComponent<UnitAgent>();
            var unitTag = unit.AddComponent<UnitTag>();
            unitTag.Id = this.Id;
            unitTag.PlayerOwer = this.PlayerOwer;
            unit.AddComponent<UnityEngine.Playables.PlayableDirector>();
            skillReleaser = unit.AddComponent<SkillReleaser>();
            skillReleaser.BindUnit(this);
        }
        if (model == null)
        {
            model = AssetManager.Instance.Instantiate(StateInfo.Model);
            model.transform.SetParent(unit.transform, false);
            model.transform.localPosition = Vector3.zero;
            model.transform.rotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
            model.name = "Model";
            if (this.PlayerOwer)
            {
                unit.AddComponent<UnitInput>();
                if (CameraSmoothFollow.Instance != null)
                    CameraSmoothFollow.Instance.SetTarget(unit.transform);
                BattleField.Instance.SetPlayerUnit(this);
            }

            unitController = unit.AddComponent<UnitController>();
            unitController.SetAttributeComponentAndStateInfo(AttributeInfo, StateInfo);
            unitController.unitId = Id;
            if (agentType == AIAgentType.TitleSprite)
            {
                var box2d = unit.AddComponent<BoxCollider2D>();
                if (aiPolicyType == AIPolicyType.Building)
                {
                    box2d.size = new Vector2(1f, 1f);
                }
                else
                {
                    box2d.size = new Vector2(0.5f, 0.3f);
                    box2d.offset = new Vector2(0f, -0.4f);
                }
                var rigibody2d = unit.AddComponent<Rigidbody2D>();
                rigibody2d.freezeRotation = true;
                rigibody2d.gravityScale = 0f;
                if (aiPolicyType == AIPolicyType.Building)
                {
                    rigibody2d.constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
        }
        if (!this.PlayerOwer)
        {
            unitController.enabled = false;
        }
        else {
            CameraController.Instance.SetTarget(unit);
        }
        if(hpBar == null && !string.IsNullOrEmpty(StateInfo.HpBar))
        {
            var barGo = AssetManager.Instance.Instantiate(StateInfo.HpBar);
            barGo.transform.SetParent(unit.transform);
            barGo.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            hpBar = barGo.GetComponent<HpBar>();
            if(hpBar == null)
                hpBar = unit.AddComponent<HpBar>();
            var hp = AttributeInfo.GetAttributeItem(AttributeType.Hp);
            var mp = AttributeInfo.GetAttributeItem(AttributeType.Mp);
            hpBar.SetHp(hp.Value.Value,hp.Value.Max);
            hpBar.SetMp(mp.Value.Value, mp.Value.Max);
            hpBar.SetHeight(StateInfo.modelHeight);
            hpBar.BindHPValueItem(hp.Value);
            hpBar.BindMPValueItem(mp.Value);
            if(unitController !=null)
                unitController.BindHpBar(hpBar);
        }
       
        unitAni = this.model.gameObject.GetComponentInChildren<UnitAnimation>();
        isDead = false;
    }
    public void OnLoopMove(){
        if (path.IsReachCurPoint(this.model.transform.position)){
            path.OnReachNextPoint();
        }
        var nextPos = path.GetNextPoint();
        if (nextPos != null)
            MoveToTargetPos(nextPos.pos);
        else
            Idle();
    }
    public void Idle(){
        SetCurTarget(null);
        StopMove();
        PlayAni(AniNameType.Idle, 1);
    }
    public bool IsHaveTarget() { return Target != null; }
    private Skill canReleaseSkill;
    private bool isReleasSkilling = false;
    public bool CanReleaserSkill() {
        if (isReleasSkilling)
            return true;
        if (isDead)
            return false;
        canReleaseSkill = skillInfo.GetCanReleaseSkill();
        return canReleaseSkill != null;
    }
    public void ReleaseCurSkill() {
        if (isReleasSkilling || isDead)
            return;
        isReleasSkilling = true;
        StopMove();
        FaceToCurTarget();
        ReleaseSkill(canReleaseSkill.Id,()=> {
            isReleasSkilling = false;
        });
    }
    public bool CurTargetInRange(float pRange){
        float oDistance;
        if (Target != null && !Target.IsDead()){
            return IsInRange(Target, pRange, out oDistance);
        }
        return false;
    }
    public bool NeedSelectTarget(List<Unit> pAllUnits)//是否需要重新选择追踪目标
    {
        float oDistance = 0;
        if (Target != null && !Target.IsDead() && IsInRange(Target, followDistance,out oDistance))
            return false;
        var unit = GetOneUnitInRange(pAllUnits, warnDistance);
        return unit != null;
    }
    public void OnDestory() {
        if (model != null){
            AssetManager.Instance.FreeGameObject(model);
            model = null;}
        if (hpBar != null){
            AssetManager.Instance.FreeGameObject(hpBar.gameObject);
            hpBar = null;}
        if (unit != null){
            GameObject.Destroy(unit);
            unit = null;}
    }
    
    public void SetCurTarget(Unit pTarget){
        Target = pTarget;
        if (pTarget != null)
            findTargetPos = StateInfo.bornPos;// this.unit.transform.position;
    }
    private void SelfAtk(System.Action pOnFinish = null) {
        var curTime = Time.realtimeSinceStartup;
        FaceToCurTarget();
        if (curTime >= nextAtkTime){
            nextAtkTime = curTime + atkCd;
            PlayAni(AniNameType.Attack, 4, true, () => {
                DoNormalAttack(null, null);
            });
            BattleField.Instance.AddDelayAction(atkCd+0.01f, pOnFinish);
        }
    }
    public void Attack(System.Action pOnFinish = null) {
        var curTime = Time.realtimeSinceStartup;
        if (curTime >= nextAtkTime){
            nextAtkTime = curTime + atkCd;
            PlayAni(AniNameType.Attack, 4, false, () => {
                PlayAni(AniNameType.Idle, 3);
                DoNormalAttack(null, null);
                if (pOnFinish != null){
                    pOnFinish.Invoke();
                }
            });
        }
    }
    public void DoNormalAttack(System.Action pAtkCB, System.Action pKillCB){
        if (Target != null){
            Target.OnDamage(atk, pAtkCB, pKillCB);
        }
    }
    public bool IsCanCampSelect(CampType pTargetCampType){
        if (campType == CampType.Monster)
        {
            return pTargetCampType != CampType.Monster && pTargetCampType != CampType.Npc;
        }
        else
        {
            return pTargetCampType != campType && pTargetCampType != CampType.Npc;
        }
    }
    private bool IsInRange(Unit pTarget, float pRange,out float oDistance)
    {
        var targetPos = pTarget.model.transform.position;
        var selfPos = this.model.transform.position;
        oDistance = Vector3.Distance(targetPos, selfPos);
        if (oDistance <= pRange)
        {
            return true;
        }
        return false;
    }
    public Unit GetOneUnitInRange(List<Unit> pAllUnits, float pRange)
    {
        Unit result = null;
        float curDistance = 9999;
        for (var i = 0; i < pAllUnits.Count; i++)
        {
            var unit_ = pAllUnits[i];
            float oDistance;
            if (!unit_.IsDead() && IsCanCampSelect(unit_.campType) && IsInRange(unit_, pRange,out oDistance))
            {
                if (oDistance < curDistance)
                {
                    curDistance = oDistance;
                    result = pAllUnits[i];
                }
            }
        }
        return result;
    }
    public void SelectAdjustTarget(List<Unit> pAllUnits)
    {
        var unit = GetOneUnitInRange(pAllUnits, warnDistance);
        SetCurTarget(unit);
    }
    //护甲减免公式
    public static float GetPhyDef(float pDefValue)
    {
        return 0.052f * pDefValue / (0.9f + 0.048f * pDefValue);
    }
    public void OnDmgEvent(Unit pAtkUnit,DamgeEvent pDmgEvent) {
        var dmgValue = pDmgEvent.value;
        if (pDmgEvent.damgeValueType == DamgeValueType.AddPhyAtk)
        {
            dmgValue += pAtkUnit.atk * pDmgEvent.effect;
        }
        OnDamage(dmgValue, null, null);
    }
    public void OnDamage(float pDamage, System.Action pAtkCB, System.Action pKillCB)
    {
        var hpValue = AttributeInfo.GetAttributeItem(AttributeType.Hp);
        var damageRe = GetPhyDef(AttributeInfo.GetAttributeItem(AttributeType.Def).Value.Value);
        var realDamage = pDamage * (1 - damageRe);
        hpValue.Value.AddValue(-realDamage);
        if (hpValue.Value.Value <= 0){
            pKillCB?.Invoke();
            OnDead();
        }
        pAtkCB?.Invoke();
    }
    private void OnDead()
    {
        Target = null;
        isDead = true;
        isReleasSkilling = false;
        StopMove();
        PlayAni(AniNameType.Dead, 6, true, () => {
            
        });
        BattleField.Instance.AddDelayAction(1,()=> {
            if (onDead != null)
            {
                onDead.Invoke(Id);
            }
            if (this.unit != null)
                this.unit.SetActive(false);
        });
        if (itemDropSetter != null) {
            var dropGroup = itemDropSetter.GetDropGroup();
            var things = dropGroup.Things;
            if (things.Count > 0) {
                var dropItemPrefab = AssetManager.Instance.Instantiate(itemDropSetter.GetDropPrefabPath());
                dropItemPrefab.transform.SetParent(BattleField.Instance.GlobalRoot.transform);
                dropItemPrefab.transform.SetPositionAndRotation(this.GetPos(), Quaternion.identity);
                var droper = dropItemPrefab.GetComponent<ItemDroper>();
                droper.thingGroup = dropGroup;
                droper.SetSprite(things[0].Icon,3);
            }
        }
    }
    public void FaceToCurTarget(){
        if (Target != null)
        {
            if (agentType == AIAgentType.NavMeshAgent)
                this.model.transform.LookAt(Target.model.transform);
            else if (agentType == AIAgentType.TitleSprite)
            {
                bool isFaceRight = Target.model.transform.position.x > this.model.transform.position.x;
                unit.transform.localScale = new Vector3(isFaceRight ? 1:-1,1,1);
                if(hpBar != null)
                    hpBar.transform.localScale = new Vector3(isFaceRight ? 0.05f : -0.05f, 0.05f, 1);
                //this.model.transform.LookAt(Target.model.transform.position, Vector3.forward);
            }
        }
    }
    public void MoveToTargetPos(Vector3 pTargetPos,System.Action pOnReach = null){
        targetPos = pTargetPos;
        if (unitAgent != null)
        {
            if (unitAgent.MoveToTaraget(targetPos, pOnReach))
            {
                PlayAni(AniNameType.Move, 3);
            }
        }
        else if (unitController != null) {
            if (unitController.MoveToTaraget(targetPos, pOnReach))
            {
                PlayAni(AniNameType.Move, 3);
            }
        }
    }
    public Vector3 GetDir() {
        if (agentType == AIAgentType.NavMeshAgent)
            return this.unit.transform.forward;
        else{
            if (this.unit.transform.localScale.x > 0)
                return Vector3.right;
            else
                return Vector3.left;
        }
    }
    public Vector3 GetPos() {return this.unit.transform.position;}
    public void MoToCurTarget(){MoveToTargetPos(Target.unit.transform.position);}
    public void StopMove() {
        if (unitAgent != null)
            unitAgent.Stop();
        else if (unitController != null)
            unitController.StopMove();
    }
    public bool NeedBackToHome()
    {
        if (IsNeedBackToHome)
            return true;
        if (Target == null)
            return false;
        if (IsNeedBackToHome == false && Target.IsDead()){
            return true;
        }
        var distance = Vector3.Distance(Target.unit.transform.position, findTargetPos);
        return distance >= followDistance;
    }
    public void BackToHome()
    {
        var distance = Vector3.Distance(this.unit.transform.position, findTargetPos);
        if (distance >= 0.2f){
            IsNeedBackToHome = true;
            SetCurTarget(null);
            MoveToTargetPos(findTargetPos);
        }
        else{
            SetCurTarget(null);
            IsNeedBackToHome = false;
        }
    }
    public void OnSecondRepeat()
    {
        if(!isDead)
            AttributeInfo.OnSecondRepeat();
    }
}
