using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAI : EnemyAI
{
    private float delayAwake = 1f;
    private float timerAwake = 0;
    private bool isAwake;

    protected override void Start()
    {
        base.Start();
        this.originScale = this.enemyCtrl.transform.localScale;
        this.originScale.x = Mathf.Abs(this.enemyCtrl.transform.localScale.x);
        this.enemyCtrl.NeverFlip.OriginScale = this.originScale;
    }

    protected override void Update()
    {
        base.Update();

        if (!this.isAwake)
        {
            //this.originScale = this.enemyCtrl.transform.localScale; 
            this.timerAwake += Time.deltaTime;
            if (this.timerAwake > this.delayAwake)
            {
                this.timerAwake = 0;
                this.isAwake = true;
            }
            return;
        }
        this.UpdateAnimation();

        if (this.MovementState == MovementState.Death)
        {
            this.col.isTrigger = true;
            this.enemyCtrl.BattleZone.DeathZone.Remove(this.enemyCtrl);
            Destroy(this.enemyCtrl.gameObject, 1f);
            return;
        }

        if (!this.enemyPlayerDetector.PlayerInArea)
        {
            this.Roaming();
        }
        else
        {
            if (Vector2.Distance(transform.position, this.enemyPlayerDetector.Player.position) <= this.distanceAttack)
            {
                this.IsStopMove = true;
                this.isColliderPlayer = true;
            }
            else
            {
                this.IsStopMove = false;
                this.isColliderPlayer = false;
            }

            this.MoveToTarget(this.enemyPlayerDetector.Player.position, this.moveSpeed);
            this.Facing(this.enemyPlayerDetector.Player.position);
        }
    }
}
