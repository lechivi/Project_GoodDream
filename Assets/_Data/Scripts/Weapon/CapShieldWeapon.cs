using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CapShieldWeapon : WeaponMelee
{
    [Header("CAP SHIELD")]
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private float throwSpeed = 10.0f;
    [SerializeField] private float throwLimitRange = 7f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float enemyDynamicDelay = 1f;

    private Rigidbody2D rb;
    private bool isThrowing;
    private bool isHitWall;
    private bool isLimitRange;

    protected override void Awake()
    {
        base.Awake();
        this.col = GetComponent<Collider2D>();
        this.rb = GetComponent<Rigidbody2D>();

        this.col.enabled = false;

        this.isReadyPrimaryMove = true;
        this.isReadySecondaryMove = true;
    }

    protected override void Update()
    {
        base.Update();
        Debug.Log(isReadyPrimaryMove);

        if (this.isThrowing )
        {
            if (Vector2.Distance(this.weaponParent.transform.position, transform.position) > this.throwLimitRange)
            {
                this.isLimitRange = true;
            }
        }
        if (this.isThrowing && (this.isHitWall || this.isLimitRange))
        {
            this.StartNewAttack();
            this.trail.emitting = true;
            this.rb.velocity = Vector2.zero;
            this.rb.bodyType = RigidbodyType2D.Dynamic;
            transform.position = Vector2.MoveTowards(transform.position, this.weaponParent.transform.position, 25f * Time.deltaTime);

            if (Vector2.Distance(this.weaponParent.transform.position, transform.position) < 0.01f)
            {
                this.ResetToHand();
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

    public override void OnClickedFirstMoveButton()
    {
        base.OnClickedFirstMoveButton();
        this.PrimaryMove();
        //Debug.Log(isReadyPrimaryMove + " " + isAttacking);
        //if (/*this.isReadyPrimaryMove && */!this.isAttacking)
        //{
            
        //}
    }

    protected override void InputSecondaryMove()
    {
        base.InputSecondaryMove();
        if (Input.GetMouseButtonDown(1) && this.isReadySecondaryMove && this.haveSkill2 && !this.isThrowing && !this.isAttacking)
        {
            if (UIManager.HasInstance)
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
            }
            this.SecondarylMove();
        }
    }

    public override void OnClickedSecondMoveButton()
    {
        base.OnClickedSecondMoveButton();
        if (this.isReadySecondaryMove && this.haveSkill2 && !this.isThrowing && !this.isAttacking)
        {
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

        this.trail.emitting = true;
        this.col.enabled = true;
        this.animator.SetTrigger("Attack");


        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1f);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("EnemyBattle"))
            {
                if (hitCollider.GetComponent<EnemyLife>() == null) return;
                Rigidbody2D enemyRb = hitCollider.GetComponent<EnemyLife>().EnemyCtrl.GetComponent<Rigidbody2D>();
                if (enemyRb != null && enemyRb.bodyType != RigidbodyType2D.Static)
                {
                    StartCoroutine(MakeEnemyDynamic(enemyRb));
                    Vector2 direction = enemyRb.transform.position - transform.position;
                    direction.Normalize();

                    enemyRb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    IEnumerator MakeEnemyDynamic(Rigidbody2D enemyRb)
    {
        enemyRb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(enemyDynamicDelay);

        enemyRb.velocity = Vector2.zero;
        enemyRb.bodyType = RigidbodyType2D.Kinematic;
        if (enemyRb.GetComponent<EnemyCtrl>().EnemyLife.Health == 0)
        {
            enemyRb.bodyType = RigidbodyType2D.Static;
        }
    }

    public void FinishAttackAnimation()
    {
        this.isAttacking = false;

        this.trail.emitting = false;
        this.col.enabled = false;
    }


    protected override void SecondarylMove()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_STAB);
        }

        this.StartNewAttack();
        this.isReadySecondaryMove = false;
        this.isAttacking = true;

        this.trail.emitting = true;

        transform.SetParent(null);

        this.animator.enabled = false;
        this.rb.bodyType = RigidbodyType2D.Dynamic;
        this.rb.AddForce(this.GetDirection() * this.throwSpeed, ForceMode2D.Impulse);
        this.isThrowing = true;
        this.col.enabled = true;
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
        this.isHitWall = false;
        this.isLimitRange = false;
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
            this.isHitWall = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
