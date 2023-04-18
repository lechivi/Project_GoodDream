using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : Weapon
{
    [Header("MELEE SETTING")]
    [SerializeField] protected float cooldownTimePrimaryMove;
    [SerializeField] protected float cooldownTimeSecondaryMove;

    protected float timerPrimaryMove;
    protected float timerSecondaryMove;
    protected bool isReadyPrimaryMove;
    protected bool isReadySecondaryMove;
    protected bool isStartCooldownPrimaryMove;
    protected bool isStartCooldownSecondaryMove;
    protected bool isAttacking;

    protected HashSet<Collider2D> hitOpponents = new HashSet<Collider2D>(); //Check opponent had been hit yet

    public float CooldownTimePrimaryMove => this.cooldownTimePrimaryMove;
    public float CooldownTimeSecondaryMove => this.cooldownTimeSecondaryMove;

    protected override void Awake()
    {
        base.Awake();
        this.WeaponType = WeaponType.Melee;

    }

    protected override void Update()
    {
        if (GameManager.HasInstance)
        {
            if (!GameManager.Instance.IsPlaying) return;
        }

        if (this.isStartCooldownPrimaryMove)
        {
            this.CooldownPrimaryMove();
        }
        if (this.isStartCooldownSecondaryMove)
        {
            this.CooldownSecondaryMove();
        }

        if (this.IsUsing && gameObject.CompareTag("PlayerWeapon"))
        {
            this.InputPrimaryMove();
            this.InputSecondaryMove();

            if (UIManager.HasInstance)
            {
                GamePanel gamePanel = UIManager.Instance.GamePanel;
                gamePanel.SecondMove.gameObject.SetActive(true);
                gamePanel.FirstMove.Icon.sprite = gamePanel.Melee1;
                gamePanel.SecondMove.Icon.sprite = gamePanel.Melee2;
            }
        }
    }

    private void CooldownPrimaryMove()
    {
        this.timerPrimaryMove += Time.deltaTime;
        if (UIManager.HasInstance && this.IsUsing && gameObject.CompareTag("PlayerWeapon"))
        {
            UIManager.Instance.GamePanel.FirstMove.StartFillMove(this.timerPrimaryMove, this.cooldownTimePrimaryMove);
        }
        if (this.timerPrimaryMove < this.cooldownTimePrimaryMove) return;

        this.timerPrimaryMove = 0;
        this.isReadyPrimaryMove = true;
        this.isStartCooldownPrimaryMove = false;
    }

    private void CooldownSecondaryMove()
    {
        this.timerSecondaryMove += Time.deltaTime;
        if (UIManager.HasInstance && this.IsUsing && gameObject.CompareTag("PlayerWeapon"))
        {
            UIManager.Instance.GamePanel.SecondMove.StartFillMove(this.timerSecondaryMove, this.cooldownTimeSecondaryMove);
        }
        if (this.timerSecondaryMove < this.cooldownTimeSecondaryMove) return;

        this.timerSecondaryMove = 0;
        this.isReadySecondaryMove = true;
        this.isStartCooldownSecondaryMove = false;
    }

    protected virtual void InputPrimaryMove()
    {
        //Only player use this method
        //for overrite
    }

    protected virtual void InputSecondaryMove()
    {
        //Only player use this method
        //for overrite
    }

    protected virtual void PrimaryMove()
    {
        //Only player use this method
        //for overrite
    }

    protected virtual void SecondarylMove()
    {
        //Only player use this method
        //for overrite
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!this.hitOpponents.Contains(collision))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && collision.gameObject.CompareTag("PlayerBattle") && gameObject.CompareTag("EnemyWeapon"))
            {
                if (collision.gameObject.GetComponent<PlayerLife>().Health <= 0) return;

                int damage = GetRandomDamage();
                collision.gameObject.GetComponent<PlayerLife>().TakeDamage(damage);

                this.hitOpponents.Add(collision);
            }

            else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && collision.gameObject.CompareTag("EnemyBattle") && gameObject.CompareTag("PlayerWeapon"))
            {
                EnemyLife enemyLife = collision.gameObject.GetComponent<EnemyLife>();
                if (enemyLife.Health <= 0) return;

                int damage = GetRandomDamage();
                enemyLife.TakeDamage(damage);

                this.hitOpponents.Add(collision);
                this.weaponParent.SpawnDamageText(damage, collision, enemyLife.EnemyCtrl.NeverFlip.transform, damage == this.maxDamage);
            }
        }
    }

    protected virtual void StartNewAttack()
    {
        this.hitOpponents.Clear();
    }
}
