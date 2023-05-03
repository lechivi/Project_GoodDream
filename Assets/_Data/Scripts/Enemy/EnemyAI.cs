using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : EnemyAbstract
{
    [Header("ENEMY AI")]
    [SerializeField] protected int contactDamage = 2;
    [SerializeField] protected float moveSpeed = 7f;
    [SerializeField] protected float roamSpeed = 2f;
    [SerializeField] protected float roamRange = 3f;
    [SerializeField] protected float delayAttackTime = 1; //Set equal with Weapon cooldown
    [SerializeField] protected float roamWaitingTime = 3f;
    [SerializeField] protected float distanceAttack = 2f;
    [SerializeField] protected float detectionObstacleRadius = 2f;
    [SerializeField, Range(0, 2)] protected int directionWeapon; //1 for Melee, 2 for Range

    public MovementState MovementState { get; set; }
    public bool IsStopMove;

    protected Collider2D col;
    protected Transform weapon;
    protected Weapon usingWeapon;
    protected EnemyPlayerDetector enemyPlayerDetector;

    protected Vector3 originScale;
    protected Vector2 targetPoint;
    protected float timerAttack = 0;
    protected float delayContactDamage = 1;
    protected float timerContactDamage = 0;
    protected bool isTargetPointSet;
    protected bool isReadyAttack = true;
    protected bool isWaiting;
    protected bool isColliderPlayer;
    protected bool isReadySendDamage;

    protected int checkLoop = 0;

    protected override void Awake()
    {
        base.Awake();
        this.col = this.enemyCtrl.GetComponent<Collider2D>();
        this.enemyPlayerDetector = GetComponentInChildren<EnemyPlayerDetector>();
        this.weapon = transform.Find("WeaponParent").transform;
        this.usingWeapon = this.weapon.GetComponentInChildren<Weapon>();
        this.originScale = this.enemyCtrl.transform.localScale;
    }

    protected virtual void Start()
    {
        this.WeaponRotation(new Vector2(this.enemyCtrl.transform.localScale.x == this.originScale.x ? this.directionWeapon : -this.directionWeapon, 1f));
        this.SetTargetPoint();
    }

    protected virtual void Update()
    {
        if (this.isColliderPlayer)
        {
            this.SendContactDamage();
        }
    }

    protected virtual void SetTargetPoint()
    {
        if (this.enemyCtrl.BattleZone == null || this.checkLoop >= 100)
        {
            this.targetPoint = new Vector2(transform.position.x + Random.Range(-roamRange, roamRange), transform.position.y + Random.Range(-roamRange, roamRange));
        }
        else
        {
            do
            {
                this.checkLoop += 1;
                if (this.checkLoop >= 100)
                {
                    Debug.LogWarning("The do-while loop in SetTargetPoint() has run too much!", transform.gameObject);
                    break;
                }

                this.targetPoint = new Vector2(transform.position.x + Random.Range(-roamRange, roamRange), transform.position.y + Random.Range(-roamRange, roamRange));
            } while (!this.enemyCtrl.BattleZone.Col.bounds.Contains(this.targetPoint));
        }
    }

    protected virtual void Facing(Vector2 target)
    {
        if (target.x > transform.position.x)
        {
            this.enemyCtrl.transform.localScale = new Vector3(-1 * this.originScale.x, this.originScale.y, this.originScale.z);
        }
        else
        {
            this.enemyCtrl.transform.localScale = this.originScale;
        }
    }

    protected virtual void WeaponRotation(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.weapon.rotation = Quaternion.Euler(0f, 0f, this.enemyCtrl.transform.localScale.x == this.originScale.x ? rotZ : rotZ - 180);
    }

    protected virtual void MoveToTarget(Vector2 target, float speed)
    {
        if (this.IsStopMove) return;
        this.enemyCtrl.transform.position = Vector2.MoveTowards(this.enemyCtrl.transform.position, target, speed * Time.deltaTime);
        this.MovementState = MovementState.Run;
    }

    protected virtual void Roaming()
    {
        if (!this.isWaiting)
        {
            if (this.enemyCtrl.BattleZone == null || this.checkLoop >= 100)
            {
                if (this.IsCollidingWithObstacle() && !this.isTargetPointSet)
                {
                    this.SetTargetPoint();
                    this.isTargetPointSet = true;
                    return;
                }
                else
                {
                    this.isTargetPointSet = false;
                }
            }

            this.MoveToTarget(this.targetPoint, this.roamSpeed);
            this.Facing(this.enemyPlayerDetector.PlayerInArea ? this.enemyPlayerDetector.Player.position : this.targetPoint);
        }

        if (Vector2.Distance(transform.position, this.targetPoint) < 0.1f)
        {
            this.isWaiting = true;
            this.MovementState = MovementState.Idle;
            Invoke("Waiting", Random.Range(0f, this.roamWaitingTime));
            this.SetTargetPoint();
        }
    }

    protected virtual void Waiting()
    {
        this.isWaiting = false;
    }

    protected virtual bool IsCollidingWithObstacle()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, this.detectionObstacleRadius);

        foreach(Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Wall")
                return true;
        }

        return false;
    }

    protected virtual void ActionWhenDetectPlayer()
    {
        //For overrite
    }

    protected virtual void AttackPlayer()
    {
        if (this.isReadyAttack)
        {
            this.WeaponRotation(-(Vector2)this.enemyPlayerDetector.Player.position + (Vector2)transform.position);

            this.isReadyAttack = false;
            this.usingWeapon.EnemyUseWeapon();
        }

        this.timerAttack += Time.deltaTime;
        if (this.timerAttack < this.delayAttackTime) return;
        this.timerAttack = 0;
        this.isReadyAttack = true;
    }

    protected virtual void UpdateAnimation()
    {
        if (this.MovementState == MovementState.Idle)
        {
            this.enemyCtrl.EnemyAnimator.Play("0_idle");

        }
        else if (this.MovementState == MovementState.Run)
        {
            this.enemyCtrl.EnemyAnimator.Play("1_Run");

        }
        else if (this.MovementState == MovementState.Death)
        {
            this.enemyCtrl.EnemyAnimator.Play("4_Death");
        }

    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ColliderWithWall") && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            this.isColliderPlayer = true;
        }    
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ColliderWithWall") && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            this.isColliderPlayer = false;
        }
    }

    protected virtual void SendContactDamage()
    {
        if (this.isReadySendDamage)
        {
            this.isReadySendDamage = false;
            if (!this.isColliderPlayer) return;
            PlayerCtrl.instance.PlayerLife.TakeDamage(this.contactDamage);
        }
        this.timerContactDamage += Time.deltaTime;
        if (this.timerContactDamage < this.delayContactDamage) return;

        this.timerContactDamage = 0f;
        this.isReadySendDamage = true;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, this.distanceAttack);
    }
}
