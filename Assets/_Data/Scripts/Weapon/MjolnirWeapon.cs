using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MjolnirWeapon : WeaponMelee
{
    public TrailRenderer trail;
    protected Rigidbody2D rb;
    protected PolygonCollider2D col;

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
        if (Input.GetMouseButtonDown(1))
        {
            if (!this.isThrowing && this.isReadySecondaryMove && !this.isAttacking)
            {
                this.StartNewAttack();
                this.isReadySecondaryMove = false;
                this.isAttacking = true;

                this.trail.emitting = true;

                transform.SetParent(null);

                this.rb.bodyType = RigidbodyType2D.Dynamic;
                this.rb.AddForce(this.GetDirection() * this.speed, ForceMode2D.Impulse);
                this.isThrowing = true;
                this.col.enabled = true;
            }
            else if (this.isThrowing)
            {
                this.StartNewAttack();
                this.rb.bodyType = RigidbodyType2D.Static;
                this.rb.bodyType = RigidbodyType2D.Dynamic;
                this.trail.emitting = true;
                this.back = true;
            }
        }

        if (this.back)
        {
            transform.position = Vector2.MoveTowards(transform.position, this.weaponParent.transform.position, 50f * Time.deltaTime);

            if (Vector2.Distance(this.weaponParent.transform.position, transform.position) < 0.01f)
            {
                transform.SetParent(this.weaponParent.transform);
                this.rb.bodyType = RigidbodyType2D.Kinematic;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler(0, 0, 90);

                this.trail.emitting = false;
                this.isStartCooldownSecondaryMove = true;
                this.isAttacking = false;
                this.isThrowing = false;
                this.back = false;
            }
        }
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
