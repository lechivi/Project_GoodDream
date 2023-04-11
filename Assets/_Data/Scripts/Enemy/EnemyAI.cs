using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform weapon;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float roamSpeed = 2f;
    [SerializeField] private float distanceAttack = 2f;
    [SerializeField] private float delayAttackTime = 2f;

    private EnemyAreaDetector enemyAreaDetector;
    private float timerAttack = 0;
    private bool isReadyAttack = true;

    [SerializeField] private Transform[] roampoints;
    private int currentRoampoint = 0;
    private bool isWaiting;

    private enum MovementState { Idle, Run, };
    private MovementState movementState;

    private void Awake()
    {
        this.enemyAreaDetector = GetComponentInChildren<EnemyAreaDetector>();
        this.WeaponRotation(new Vector2(this.enemy.localScale.x == -1 ? 0.8f : -0.8f, 1f));
    }

    private void Update()
    {
        if (this.enemyAreaDetector.PlayerInArea)
        {
            this.Facing(this.enemyAreaDetector.Player.position);

            if (this.GetDistanceFromPlayer() > this.distanceAttack)
            {
                this.MoveToPlayer();
                this.WeaponRotation(new Vector2(this.enemy.localScale.x == -1 ? 0.8f : -0.8f, 1f));
            }
            else
            {
                this.AttackPlayer();
            }
        }
        else
        {
            this.Roaming();
        }
        
        if (!this.isReadyAttack)
        {
            this.timerAttack += Time.deltaTime;
            if (this.timerAttack < this.delayAttackTime) return;
            this.timerAttack = 0;
            this.isReadyAttack = true;
        }

        this.UpdateAnimation();
    }

    private void MoveToPlayer()
    {
        this.enemy.position = Vector2.MoveTowards(this.enemy.position, this.enemyAreaDetector.Player.position, this.moveSpeed * Time.deltaTime);
        this.movementState = MovementState.Run;
    }

    private float GetDistanceFromPlayer()
    {
        return Vector2.Distance(this.enemy.position, this.enemyAreaDetector.Player.position);
    }

    private void Facing(Vector2 target)
    {
        if (target.x > this.enemy.position.x)
        {
            this.enemy.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            this.enemy.localScale = Vector3.one;
        }
    }

    private void WeaponRotation(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.weapon.rotation = Quaternion.Euler(0f, 0f, this.enemy.localScale.x == -1 ? rotZ : rotZ - 180);
    }

    private void AttackPlayer()
    {
        if (this.isReadyAttack)
        {
            this.WeaponRotation((Vector2)this.enemyAreaDetector.Player.position - (Vector2)transform.position);

            this.isReadyAttack = false;
            this.weaponAnimator.Play("Sword_Attack");
        }

        this.timerAttack += Time.deltaTime;
        if (this.timerAttack < this.delayAttackTime) return;
        this.timerAttack = 0;
        this.isReadyAttack = true;
    }

    private void Roaming()
    {
        if (Vector2.Distance(this.roampoints[this.currentRoampoint].transform.position, transform.position) < 0.1f)
        {
            this.isWaiting = true;
            this.movementState = MovementState.Idle;
            Invoke("Waited", UnityEngine.Random.Range(0f, 4f));

            this.currentRoampoint++;
            if (this.currentRoampoint >= this.roampoints.Length)
            {
                this.currentRoampoint = 0;
            }
        }
        this.Facing(this.roampoints[this.currentRoampoint].transform.position);

        if (!isWaiting)
        {
            this.enemy.transform.position = Vector2.MoveTowards(this.enemy.transform.position, this.roampoints[this.currentRoampoint].transform.position, this.roamSpeed * Time.deltaTime);
            this.movementState = MovementState.Run;
        }
    }

    private void Waited()
    {
        this.isWaiting = false;
    }

    private void UpdateAnimation()
    {
        if (this.movementState == MovementState.Idle)
        {
            this.enemyAnimator.Play("0_idle");
        }
        else if (this.movementState == MovementState.Run)
        {
            this.enemyAnimator.Play("1_Run");
        }
    }
}
