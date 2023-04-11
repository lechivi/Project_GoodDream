using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponMelee : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float speedAttack = 5f;

    public TrailRenderer trail;

    public float cooldownTimePrimaryMove;
    public float cooldownTimeSpecialMove;

    public bool isReadyPrimaryMove { get; set; }
    public bool isReadySpecialMove { get; set; }

    public bool isStartCooldownPrimaryMove { get; set; }
    public bool isStartCooldownSpecialMove { get; set; }

    protected WeaponParent weaponParent;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected PolygonCollider2D col;

    private float speed = 9f;
    private bool isAttacking;
    private bool isThrowing; //for distinguish 2 phase of special move
    private bool back; //for second phase of special move

    protected virtual void Awake()
    {
        this.weaponParent = transform.parent.GetComponent<WeaponParent>();
        this.animator = GetComponent<Animator>();
        this.rb = GetComponent<Rigidbody2D>();
        this.col = GetComponent<PolygonCollider2D>();

        this.animator.enabled = false;
        this.col.enabled = false;
        this.trail.emitting = false;

        this.isReadyPrimaryMove = true;
        this.isReadySpecialMove = true;
    }

    protected virtual void Update()
    {
        this.PrimaryMove();
        this.SpecialMove();
    }

    protected virtual void PrimaryMove()
    {
        if (Input.GetMouseButtonDown(0) && !this.isAttacking && this.isReadyPrimaryMove)
        {
            this.isReadyPrimaryMove = false;
            this.isStartCooldownPrimaryMove = true;
            this.isAttacking = true;

            this.animator.enabled = true;
            this.trail.emitting = true;
            this.col.enabled = true;

            this.animator.SetTrigger("Attack");
        }
    }

    public void FinishAnimation()
    {
        this.isAttacking = false;

        this.animator.enabled = false;
        this.trail.emitting = false;
        this.col.enabled = false;
    }

    protected virtual void SpecialMove()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!this.isThrowing && !this.isAttacking && this.isReadySpecialMove)
            {
                this.isReadySpecialMove = false;
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
                this.isStartCooldownSpecialMove = true;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.isThrowing && collision.CompareTag("Wall"))
        {
            this.trail.emitting = false;

            this.rb.bodyType = RigidbodyType2D.Static;

        }

        if (collision.CompareTag("Enemy_Battle"))
        {
            collision.GetComponentInChildren<EnemyLife>().TakeDamage(damage);
            Debug.Log("Enemy hit " + damage);
        }
    }
}
