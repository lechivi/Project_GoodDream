using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealerAI : EnemyAI
{
    [Header("ENEMY HEALER")]
    [SerializeField] private GameObject healTextPrefab;
    [SerializeField] private GameObject animationHealPrefab;
    [SerializeField] private int healAmout = 10;
    [SerializeField] private float delayHealTime = 1f;
    [SerializeField] private float delayCooldownHealTime = 1f;
    [SerializeField] private float delayRunAwayTime = 1f;

    private EnemyAllyDetector enemyAllyDetector;

    private float timerHeal = 0f;
    private float timerCooldownHeal = 0f;
    private float timerRunAway = 0f;
    private bool isStartAction;
    private bool isReadyHealSkill = true;
    private bool isStartCooldownHealSkill;
    private bool isHealing;

    protected override void Awake()
    {
        base.Awake();
        this.enemyAllyDetector = GetComponentInChildren<EnemyAllyDetector>();
    }

    protected override void Start()
    {
        base.Start();
        //this.delayAttackTime = this.usingWeapon.GetComponent<WeaponMagic>().CooldownTimeAttackMove;
    }

    protected override void Update()
    {
        base.Update();
        this.UpdateAnimation();

        if (this.MovementState == MovementState.Death)
        {
            this.col.isTrigger = true;
            return;
        }

        if (this.isStartCooldownHealSkill)
        {
            this.CooldownHealSkill();
        }

        if (this.enemyAllyDetector.CheckLowestHealthAlly() && this.isReadyHealSkill)
        {
            Debug.Log("Heal");
            this.isReadyHealSkill = false;
            this.isStopMove = true;
            this.MovementState = MovementState.Idle;
            this.HealSkill();
        }
        else
        {
            if (!this.enemyCtrl.BattleZone.Col.bounds.Contains(this.enemyCtrl.transform.position)) //When hit wall because of running away from Player, Healer enemy stop move
            {
                //Debug.Log("Hit Wall");

                this.isStopMove = true;
                this.MovementState = MovementState.Idle;
                if (this.enemyPlayerDetector.PlayerInArea) //If Player in AreaDetector, Healer enemy facing to Player
                {
                    this.Facing(this.enemyPlayerDetector.Player.position);

                    if (Vector2.Distance(transform.position, this.enemyPlayerDetector.Player.position) <= this.distanceAttack) //Attack when Player enter AttackZone
                    {
                        this.AttackPlayer();
                    }
                }
                else //If Player exit AreaDetector, act casual
                {
                    this.isStartAction = false;
                    this.timerRunAway += Time.deltaTime;
                    if (this.timerRunAway < this.delayRunAwayTime) return;
                    this.timerRunAway = 0f;

                    this.SetTargetPoint();
                    this.MoveToTarget(targetPoint, this.roamSpeed);
                    this.Facing(targetPoint);
                }

            }
            else
            {
                if (!this.enemyPlayerDetector.PlayerInArea)
                {
                    if (!this.isStartAction) //Roaming around when not detect Player
                    {
                        this.Roaming();
                    }

                    else //Still run to obstacle in a second (timerRunAway) after Player exit AreaDetector
                    {
                        this.MoveToTarget(targetPoint, this.moveSpeed);
                        this.Facing(targetPoint);

                        this.timerRunAway += Time.deltaTime;
                        if (this.timerRunAway < this.delayRunAwayTime) return;
                        this.timerRunAway = 0f;

                        this.isStartAction = false;
                        this.MovementState = MovementState.Idle;
                    }
                }

                else //Run away from Player when Player enter AreaDetector
                {
                    if (!this.enemyCtrl.BattleZone.IsPlayerEnter) return;

                    if (!this.enemyCtrl.BattleZone.IsPlayerEnter) return;
                    this.isStartAction = true;
                    this.targetPoint = this.enemyCtrl.transform.position * 2 - this.enemyPlayerDetector.Player.position;
                    this.MoveToTarget(targetPoint, this.moveSpeed);
                    this.Facing(targetPoint);
                }

            }
        }
    }

    private void CooldownHealSkill()
    {
        this.timerCooldownHeal += Time.deltaTime;
        if (this.timerCooldownHeal < this.delayCooldownHealTime) return;

        this.timerCooldownHeal = 0;
        this.isReadyHealSkill = true;
        this.isStartCooldownHealSkill = false;
    }

    private void HealSkill()
    {
        this.enemyAllyDetector.LowestHealthAlly.Heal(this.healAmout);
        Instantiate(this.animationHealPrefab, this.enemyAllyDetector.LowestHealthAlly.transform.position, Quaternion.identity, this.enemyAllyDetector.LowestHealthAlly.transform);
        this.SpawnHealText(this.healAmout, this.enemyAllyDetector.LowestHealthAlly.EnemyCtrl.NeverFlip.transform);

        this.MovementState = MovementState.Magic;
        this.usingWeapon.GetComponent<Animator>().SetTrigger("Magic");
        Invoke("EndHealSkill", 1.2f);
    }

    private void SpawnHealText(int heal, Transform ally)
    {
        Vector3 position = new Vector3(ally.position.x + Random.Range(-0.5f, 0.5f), ally.position.y + 1.4f, ally.position.z);
        GameObject healTextObject = Instantiate(this.healTextPrefab, position, Quaternion.identity, ally);

        healTextObject.GetComponentInChildren<TextMesh>().text = "+" + heal.ToString();
        healTextObject.GetComponentInChildren<TextMesh>().color = Color.green;
        healTextObject.GetComponentInChildren<TextMesh>().fontSize = 75;

        //Debug.Log(heal, healTextObject.gameObject);
        StartCoroutine(DestroyHealText(healTextObject));
    }

    protected virtual IEnumerator DestroyHealText(GameObject damageTextObject)
    {
        yield return new WaitForSeconds(1f);
        Destroy(damageTextObject);
    }

    private void EndHealSkill()
    {
        this.isStartCooldownHealSkill = true;
        this.isStopMove = false;
    }

    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();

        if (this.MovementState == MovementState.Magic)
        {
            this.enemyCtrl.EnemyAnimator.Play("2_Attack_Magic");
        }
    }


}
