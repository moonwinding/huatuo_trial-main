using EventG;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public float speed;
    public int unitId;

    private AttributeComponent attributeComponent;
    private StateInfo stateInfo;
    private bool isMove;
    private bool isAtk;
    private Vector3 move;
    private AnimationController animationcontroller;
    private Vector3 pos_;
    private Vector3 targetPos_;
    private bool isMoveToTarget;
    private System.Action onAtkFinish;
    private HpBar hpBar;
    private Vector3 oriScale;
    private Rigidbody2D rigiboDody2d;
    public CampType CampType { get { return stateInfo.campType; } }
    public void SetAttributeComponentAndStateInfo(AttributeComponent pAttributeComponent, StateInfo pStateInfo) {
        attributeComponent = pAttributeComponent;
        stateInfo = pStateInfo;
    }
    public void BindHpBar(HpBar pHpBar) {
        hpBar = pHpBar;
        oriScale = hpBar.transform.localScale;
    }
    void Start(){
        animationcontroller = this.gameObject.GetComponent<AnimationController>();
        if(animationcontroller == null)
            animationcontroller = this.gameObject.AddComponent<AnimationController>();

        speed = attributeComponent.GetAttributeItem( AttributeType.MoveSpeed).Value.GetValue();
        pos_ = this.transform.localPosition;
        rigiboDody2d = this.gameObject.GetComponent<Rigidbody2D>();
    }
    public bool MoveToTaraget(Vector3 pTaragetPos,System.Action pOnReach) {
        if (Vector3.Distance(pos_, pTaragetPos) <= 0.1f){
            pOnReach?.Invoke();
            isMoveToTarget = false;
            return false;
        }
        else {
            targetPos_ = pTaragetPos;
            isMoveToTarget = true;
            return true;
        }
    }
    public void SetMoveDir(Vector2 pMoveDir) {
        if (pMoveDir.magnitude > 0)
        {
            Debug.LogError($"pMoveDir{pMoveDir}");
            //move = pMoveDir;
            if (Mathf.Abs(pMoveDir.y) > Mathf.Abs(pMoveDir.x))
            {
                if (pMoveDir.y > 0)
                {
                    move = this.transform.forward;
                }
                else {
                    move = -this.transform.forward;
                }
                
            }
            else {
                if (pMoveDir.x > 0)
                {
                    move = this.transform.right;
                }
                else
                {
                    move = -this.transform.right;
                }
            }
            //var moveDir = (this.transform.forward * pMoveDir.y + this.transform.right * pMoveDir.x);// new Vector3(pMoveDir.x,0, pMoveDir.y);
            //move = moveDir;// new Vector3(pMoveDir.x,0, pMoveDir.y);
            Debug.LogError($"move{move}");
        }
        else
        {
            move = Vector3.zero;
        }
    }
    public void StopMove() {
        isMoveToTarget = false;
        move = Vector3.zero;
    }

    public void Atk() {
        isAtk = true;
        onAtkFinish = ()=> {
            var dmageEvent = new DamgeEvent();
            dmageEvent.owerId = unitId;
            dmageEvent.areaType = AreaType.ForwardAngle;
            dmageEvent.dmgeType = DmageType.Phy;
            dmageEvent.damgeValueType = DamgeValueType.AddPhyAtk;
            dmageEvent.targetType = TargetSelectType.Enemy;
            var atkUnit = BattleField.Instance.GetUnit(unitId);
            dmageEvent.range = atkUnit.atkDistance;
            dmageEvent.effect = 1f;
            dmageEvent.angle = 60f;
            BattleEnumEvent.SendMessage(BattleEventType.Damg, dmageEvent);
        };
    }
    void Update()
    {
        if (isMoveToTarget)
        {
            if (isAtk)
            {
                move = Vector3.zero;
            }
            else
            {
                if (Vector3.Distance(pos_, targetPos_) <= 0.1f)
                {
                    isMoveToTarget = false;
                }
                else
                {
                    move = Vector3.Normalize(targetPos_ - pos_);
                }
            }
        }
        this.transform.Translate(move * speed * Time.deltaTime);
        //rigiboDody2d.MovePosition(pos_ + move * speed * Time.deltaTime);
        pos_ = this.transform.position;
        isMove = move.x != 0 || move.z != 0 || move.y != 0;
        if (!isAtk){
            if (isMove)
                animationcontroller.SetAniType(AniNameType.Move, 3, false, null);
            else
                animationcontroller.SetAniType(AniNameType.Idle, 3, false, null);
        }
        else
            animationcontroller.SetAniType(AniNameType.Attack,4, false, ()=> {
                animationcontroller.SetAniType(AniNameType.Idle, 3, true, null);
                onAtkFinish?.Invoke();
                isAtk = false;
            });
        if (isMove && move.x != 0)
        {
            var isFaceRight = move.x > 0;
            this.transform.localScale = new Vector3(isFaceRight ? 1 : -1, 1, 1);
            var x = (isFaceRight ? 1f : -1f) * oriScale.x;
            hpBar.transform.localScale = new Vector3(x, oriScale.y,oriScale.z);
        }  
    }
    private void OnChangeAnimator(AniNameType pAniNameType , int pProx, bool pIsForce, System.Action pOnFinish) {
        if (animationcontroller != null) {
            animationcontroller.SetAniType(pAniNameType, pProx, pIsForce, pOnFinish);
        }
    }
}
