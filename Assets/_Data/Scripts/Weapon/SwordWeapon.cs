using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwordWeapon : WeaponMelee
{
    [Header("SWORD")]
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private float powerStabDistance = 1.0f;
    [SerializeField] private float powerStabDuration = 0.5f;
    [SerializeField] private float powerStabSpeed = 10.0f;

    private bool isPowerStabbing = false;
    private float powerStabTimer = 0.0f;
    private Vector3 powerStabStartPos;
    private Vector3 powerStabEndPos;

    protected override void Awake()
    {
        base.Awake();
        this.animator = GetComponent<Animator>();
        this.col = GetComponent<Collider2D>();

        this.col.isTrigger = true;
        this.col.enabled = false;

        this.isReadyPrimaryMove = true;
        this.isReadySecondaryMove = true;
        //this.IsUsing = true; //TEST
    }

    protected override void Update()
    {
        base.Update();
        if (this.isPowerStabbing)
        {
            this.powerStabTimer += Time.deltaTime;
            float t = Mathf.Clamp01(this.powerStabTimer / this.powerStabDuration);
            this.weaponParent.PlayerCtrl.transform.position = Vector3.Lerp(this.powerStabStartPos, this.powerStabEndPos, t);

            if (t >= 1.0f)
            {
                this.FinishAttackAnimation();
                this.isPowerStabbing = false;
                this.powerStabTimer = 0;      
            }

        }
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
        this.trail.emitting = true;

        this.animator.SetTrigger("Attack");
    }


    protected override void SecondarylMove()
    {
        base.SecondarylMove();
        this.StartNewAttack();
        this.isReadySecondaryMove = false;
        this.isStartCooldownSecondaryMove = true;
        this.isAttacking = true;

        this.col.enabled = true;
        this.trail.emitting = true;

        if (!this.isPowerStabbing)
        {
            int direction = this.weaponParent.PlayerCtrl.PlayerMovement.IsFacingRight ? 1 : -1;
            this.isPowerStabbing = true;
            this.powerStabStartPos = this.weaponParent.PlayerCtrl.transform.position;

            Vector2 powerStabDirection = this.weaponParent.transform.right * direction;
            RaycastHit2D hit = Physics2D.Raycast(this.powerStabStartPos, powerStabDirection, this.powerStabDistance, LayerMask.GetMask("Wall"));
            if (hit.collider != null)
            {
                this.powerStabEndPos = hit.point;
            }
            else
            {
                this.powerStabEndPos = this.weaponParent.PlayerCtrl.transform.position + this.weaponParent.transform.right * direction * powerStabDistance;
            }

            float distance = Vector3.Distance(this.powerStabStartPos, this.powerStabEndPos);
            this.powerStabDuration = distance / this.powerStabSpeed;

            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySFX(AUDIO.SFX_STAB);
            }
        }
    }

    public void FinishAttackAnimation()
    {
        this.isAttacking = false;

        this.col.enabled = false;
        this.trail.emitting = false;
    }

    public override void EnemyUseWeapon()
    {
        base.EnemyUseWeapon();
        this.StartNewAttack();
        this.PrimaryMove();
    }
}
