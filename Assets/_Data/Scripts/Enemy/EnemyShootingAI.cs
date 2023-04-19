using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingAI : EnemyAI
{
    [Header("ENEMY SHOOTING")]
    [SerializeField] private int maxAmmo = 4;
    [SerializeField] private float reloadTime = 2f;
    [SerializeField] private float fireRate = 2f;

    private int currentAmmo;
    private float timerReload = 0f;
    private float waitForNextShot;
    private bool isReloading;


    protected override void Awake()
    {
        base.Awake();
        this.currentAmmo = this.maxAmmo;
    }

    protected override void Update()
    {
        base.Update();
        this.UpdateAnimation();

        if (this.MovementState == MovementState.Death)
        {
            this.col.isTrigger = true;
            return;
        }

        this.Roaming();

        if (this.enemyPlayerDetector.PlayerInArea)
        {
            this.Facing(this.enemyPlayerDetector.Player.position);
            this.WeaponRotation(-(Vector2)this.enemyPlayerDetector.Player.position + (Vector2)transform.position);

            this.HandleShooting();
        }
        else
        {
            this.WeaponRotation(new Vector2(this.enemyCtrl.transform.localScale.x == 1 ? this.directionWeapon : -this.directionWeapon, 1f));
        }

        if (this.isReloading)
        {
            this.ReloadBullet();
        }

    }

    protected override void Roaming()
    {
        if (!this.isWaiting)
        {
            if (this.enemyCtrl.BattleZone == null || this.checkLoop >= 100)
            {
                if (this.IsCollidingWithObstacle() && !this.isTargetPointSet)
                {
                    this.SetTargetPoint();
                    this.isTargetPointSet = true;
                    return;
                }
                else
                {
                    this.isTargetPointSet = false;
                }
            }

            this.MoveToTarget(this.targetPoint, this.roamSpeed);
            this.Facing(this.enemyPlayerDetector.PlayerInArea ? this.enemyPlayerDetector.Player.position : this.targetPoint);
        }

        if (Vector2.Distance(transform.position, this.targetPoint) < 0.1f)
        {
            this.isWaiting = true;
            this.MovementState = MovementState.Idle;
            Invoke("Waiting", Random.Range(0f, this.roamWaitingTime));
            this.SetTargetPoint();
        }
    }
    private void HandleShooting()
    {
        if (this.isReloading) return;

        if (Time.time > this.waitForNextShot)
        {
            this.waitForNextShot = Time.time + 1f / this.fireRate;

            if (this.currentAmmo > 0)
            {
                this.currentAmmo--;
                this.usingWeapon.GetComponent<WeaponShooting>().Direction = this.enemyCtrl.transform.localScale == Vector3.one ? -1 : 1;
                this.usingWeapon.EnemyUseWeapon();
            }
            else
            {
                this.isReloading = true;
            }
        }
    }

    private void ReloadBullet()
    {
        this.timerReload += Time.deltaTime;
        if (this.timerReload < this.reloadTime) return;

        this.timerReload = 0;
        this.currentAmmo = this.maxAmmo;
        this.isReloading = false;
    }

}
