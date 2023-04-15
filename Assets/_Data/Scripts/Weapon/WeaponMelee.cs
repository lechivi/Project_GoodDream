using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : Weapon
{
    [Header("MELEE SETTING")]
    [SerializeField] protected float cooldownTimePrimaryMove;
    [SerializeField] protected float cooldownTimeSpecialMove;

    protected bool isReadyPrimaryMove { get; set; }
    protected bool isReadySpecialMove { get; set; }

    protected bool isStartCooldownPrimaryMove { get; set; }
    protected bool isStartCooldownSpecialMove { get; set; }

    protected bool isAttacking;

    protected float timerPrimaryMove;
    protected float timerSpecialMove;

    protected HashSet<Collider2D> hitOpponents = new HashSet<Collider2D>(); //Check opponent had been hit yet

    protected override void Update()
    {
        if (this.isStartCooldownPrimaryMove)
        {
            this.CooldownPrimaryMove();
        }
        if (this.isStartCooldownSpecialMove)
        {
            this.CooldownSpecialMove();
        }

        if (this.IsUsing && gameObject.CompareTag("PlayerWeapon"))
        {
            this.InputPrimaryMove();
            this.InputSpecialMove();
        }
    }

    private void CooldownPrimaryMove()
    {
        this.timerPrimaryMove += Time.deltaTime;
        if (this.timerPrimaryMove < this.cooldownTimePrimaryMove) return;

        this.timerPrimaryMove = 0;
        this.isReadyPrimaryMove = true;
        this.isStartCooldownPrimaryMove = false;
    }

    private void CooldownSpecialMove()
    {
        this.timerSpecialMove += Time.deltaTime;
        if (this.timerSpecialMove < this.cooldownTimeSpecialMove) return;

        this.timerSpecialMove = 0;
        this.isReadySpecialMove = true;
        this.isStartCooldownSpecialMove = false;
    }

    protected virtual void InputPrimaryMove()
    {
        //Only player use this method
        //for overrite
    }

    protected virtual void InputSpecialMove()
    {
        //Only player use this method
        //for overrite
    }

    protected virtual void PrimaryMove()
    {
        //Only player use this method
        //for overrite
    }

    protected virtual void SpecialMove()
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
