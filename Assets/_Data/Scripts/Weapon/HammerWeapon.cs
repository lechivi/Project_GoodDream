using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HammerWeapon : WeaponMelee
{
    public TrailRenderer trail;
    protected Rigidbody2D rb;

    private float speed = 9f;
    private bool isThrowing; //for distinguish 2 phase of special move
    private bool back; //for second phase of special move

    protected override void Awake()
    {
        base.Awake();

        this.animator = GetComponent<Animator>();
        this.rb = GetComponent<Rigidbody2D>();
        this.col = GetComponent<PolygonCollider2D>();

        this.col.enabled = false;
        this.trail.emitting = false;

        this.isReadyPrimaryMove = true;
        this.isReadySecondaryMove = true;
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

        this.trail.emitting = true;
        this.col.enabled = true;

        this.animator.SetTrigger("Attack");

    }

    public void FinishAnimation()
    {
        this.isAttacking = false;

        this.trail.emitting = false;
        this.col.enabled = false;
    }

    protected override void InputSecondaryMove()
    {
        base.InputSecondaryMove();
        this.SecondarylMove();
    }

    protected override void SecondarylMove()
    {
        if (Input.GetMouseButtonDown(1) && this.haveSkill2)
        {
            if (UIManager.HasInstance)
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
            }

            if (!this.isThrowing && this.isReadySecondaryMove && !this.isAttacking)
            {
                if (AudioManager.HasInstance)
                {
                    AudioManager.Instance.PlaySFX(AUDIO.SFX_HAMMERTHROW);
                }

                this.StartNewAttack();
                this.isReadySecondaryMove = false;
                this.isAttacking = true;

                this.trail.emitting = true;

                transform.SetParent(null);

                this.animator.enabled = false;
                this.rb.bodyType = RigidbodyType2D.Dynamic;
                this.rb.AddForce(this.GetDirection() * this.speed, ForceMode2D.Impulse);
                this.isThrowing = true;
                this.col.enabled = true;

            }
            else if (this.isThrowing)
            {
                this.StartNewAttack();
                this.trail.emitting = true;
                this.rb.bodyType = RigidbodyType2D.Dynamic;
                this.back = true;
            }
        }

        if (this.back)
        {
            this.rb.velocity = Vector2.zero;
            transform.position = Vector2.MoveTowards(transform.position, this.weaponParent.transform.position, 50f * Time.deltaTime);

            if (Vector2.Distance(this.weaponParent.transform.position, transform.position) < 0.01f)
            {
                this.ResetToHand();
            }
        }
    }

    public override void SetActiveWeapon(bool active)
    {
        base.SetActiveWeapon(active);
        if (!active)
        {
            this.ResetToHand();
        }
    }

    protected virtual void ResetToHand()
    {
        transform.SetParent(this.weaponParent.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 90);
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        
        this.animator.enabled = true;
        this.rb.bodyType = RigidbodyType2D.Kinematic;
        this.trail.emitting = false;
        this.isStartCooldownSecondaryMove = true;
        this.isAttacking = false;
        this.isThrowing = false;
        this.back = false;
    }

    protected virtual Vector2 GetDirection()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePos - (Vector2)this.weaponParent.transform.position;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (this.isThrowing && collision.CompareTag("Wall"))
        {
            this.trail.emitting = false;
            this.rb.bodyType = RigidbodyType2D.Static;
        }
    }
}
