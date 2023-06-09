using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerFistWeapon : WeaponMelee
{
    [Header("POWER FIST")]
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float enemyDynamicDelay = 1f;

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
        col = GetComponent<Collider2D>();

        this.col.enabled = false;

        this.isReadyPrimaryMove = true;
        this.isReadySecondaryMove = true;
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
                this.isAttacking = false;
                this.trail.emitting = false;

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

    public override void OnClickedFirstMoveButton()
    {
        base.OnClickedFirstMoveButton();
        Debug.Log(this.isReadyPrimaryMove);
        if (this.isReadyPrimaryMove && !this.isAttacking)
        {
            Debug.Log("In");
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

    public override void OnClickedSecondMoveButton()
    {
        base.OnClickedSecondMoveButton();
        if (this.isReadySecondaryMove && !this.isAttacking && this.haveSkill2)
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

    protected override void SecondarylMove()
    {
        base.SecondarylMove();
        this.trail.emitting = true;
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_STAB);
        }

        this.StartNewAttack();
        this.isReadySecondaryMove = false;
        this.isStartCooldownSecondaryMove = true;
        this.isAttacking = true;

        //this.col.enabled = true;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
