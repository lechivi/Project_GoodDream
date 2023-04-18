using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAI : EnemyAI
{
    //[Header("ENEMY MELEE")]

    protected override void Start()
    {
        base.Start();
        this.delayAttackTime = this.usingWeapon.GetComponent<WeaponMelee>().CooldownTimePrimaryMove;
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

        if (!this.enemyPlayerDetector.PlayerInArea)
        {
            this.Roaming();
        }
        else
        {
            if (!this.enemyCtrl.BattleZone.IsPlayerEnter) return;
            this.ActionWhenDetectPlayer();
        }
    }

    protected override void ActionWhenDetectPlayer()
    {
        base.ActionWhenDetectPlayer();
        this.Facing(this.enemyPlayerDetector.Player.position);

        if (Vector2.Distance(transform.position, this.enemyPlayerDetector.Player.position) > this.distanceAttack) //Compare distance-from-player with distanceAttack
        {
            this.MoveToTarget(this.enemyPlayerDetector.Player.position, Random.Range(this.moveSpeed - 2, this.moveSpeed + 2));
            this.WeaponRotation(new Vector2(this.enemyCtrl.transform.localScale.x == 1 ? this.directionWeapon : -this.directionWeapon, 1f));
        }
        else
        {
            this.AttackPlayer();
        }
    }
}
