using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TridentWeapon : WeaponMelee
{
    [Header("TRIDENT")]
    [SerializeField] protected List<TrailRenderer> trails = new List<TrailRenderer>();
    [SerializeField] protected GameObject skillWhirlPoolPrefab;

    protected override void Awake()
    {
        base.Awake();
        col = GetComponent<Collider2D>();

        this.col.enabled = false;

        this.isReadyPrimaryMove = true;
        this.isReadySecondaryMove = true;
        //this.IsUsing = true; //TEST
    }

    protected override void InputPrimaryMove()
    {
        base.InputPrimaryMove();
        if (Input.GetMouseButtonDown(0) && this.isReadyPrimaryMove && !this.isAttacking)
        {
            if (UIManager.HasInstance)
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
            }
            this.PrimaryMove();
        }
    }

    protected override void InputSecondaryMove()
    {
        base.InputSecondaryMove();
        if (Input.GetMouseButtonDown(1) && this.isReadySecondaryMove && !this.isAttacking && this.haveSkill2)
        {
            if (UIManager.HasInstance)
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
            }
            this.SecondarylMove();
        }
    }

    protected override void PrimaryMove()
    {
        base.PrimaryMove();
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_SLASH);
        }

        this.StartNewAttack();
        this.isReadyPrimaryMove = false;
        this.isStartCooldownPrimaryMove = true;
        this.isAttacking = true;

        this.col.enabled = true;
        this.SetActiveTrail(true);

        this.animator.SetTrigger("Attack");
    }

    protected override void SecondarylMove()
    {
        base.SecondarylMove();
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_SKILL_WHIRLPOOL);
        }

        this.StartNewAttack();
        this.isReadySecondaryMove = false;
        this.isStartCooldownSecondaryMove = true;
        this.isAttacking = true;

        this.animator.SetTrigger("Attack");
        Instantiate(this.skillWhirlPoolPrefab, transform.position, Quaternion.identity, this.weaponParent.SpawnPool);
    }

    public void FinishAnimation()
    {
        this.isAttacking = false;

        this.SetActiveTrail(false);
        this.col.enabled = false;
    }

    protected void SetActiveTrail(bool active)
    {
        foreach (TrailRenderer trail in trails)
        {
            trail.emitting = active;
        }
    }
}
