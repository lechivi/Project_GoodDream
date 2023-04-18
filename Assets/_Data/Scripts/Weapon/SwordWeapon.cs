using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordWeapon : WeaponMelee
{
    [Header("SWORD SETTING")] 
    [SerializeField] private GameObject slashEffectPrefab;
    [SerializeField] private Transform slashEffectPoint;

    private BoxCollider2D col;

    protected override void Awake()
    {
        base.Awake();
        this.animator = GetComponent<Animator>();
        this.col = GetComponent<BoxCollider2D>();

        this.col.isTrigger = true;
        this.col.enabled = false;
    }

    protected override void InputPrimaryMove()
    {
        base.InputPrimaryMove();
        if (Input.GetMouseButtonDown(0))
        {
            this.PrimaryMove();
        }
    }
    protected override void PrimaryMove()
    {
        base.PrimaryMove();
        this.StartNewAttack();
        this.isReadyPrimaryMove = false;
        this.isStartCooldownPrimaryMove = true;
        this.isAttacking = true;

        this.col.enabled = true;

        this.animator.SetTrigger("Attack");
        //GameObject slashEffect = Instantiate(this.slashEffectPrefab, this.slashEffectPoint.position, Quaternion.identity, transform);
        //this.slashEffectPrefab.SetActive(true);
        //Invoke("SetFXFalse", 0.5f);
    }

    //private void SetFXFalse()
    //{
    //    this.slashEffectPrefab.SetActive(false);
    //}

    public void FinishAttackAnimation()
    {
        this.isAttacking = false;

        this.col.enabled = false;
    }

    public override void EnemyUseWeapon()
    {
        base.EnemyUseWeapon();
        this.StartNewAttack();
        this.PrimaryMove();
    }
}
