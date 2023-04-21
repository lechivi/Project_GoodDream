using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNecromancerAI : EnemyAI
{
    [Header("NECROMANCER")]
    [SerializeField] protected GameObject skeletonPrefab;
    [SerializeField] protected GameObject animationRaiseDeadPrefab;
    [SerializeField] protected int amountSummon;
    [SerializeField] protected bool isSummonAll;

    protected bool isUsingSkill;
    protected bool checkSkill;

    protected int checkLoop2;

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
            if (!this.enemyCtrl.BattleZone.IsPlayerEnter) return;
            this.ActionWhenDetectPlayer();
        }
        else
        {
            this.WeaponRotation(new Vector2(this.enemyCtrl.transform.localScale.x == this.originScale.x ? this.directionWeapon : -this.directionWeapon, 1f));
        }

        EnemyLife enemyLife = this.enemyCtrl.EnemyLife;
        if (enemyLife.Health <= enemyLife.MaxHealth * 2 / 3 && !this.checkSkill)
        {
            this.isUsingSkill = true;
            this.checkSkill = true;
            this.RaiseTheDead();
            Invoke("FinishUseSkill", 1f);
        }
    }

    protected override void ActionWhenDetectPlayer()
    {
        base.ActionWhenDetectPlayer();
        this.Facing(this.enemyPlayerDetector.Player.position);
        this.WeaponRotation(-(Vector2)this.enemyPlayerDetector.Player.position + (Vector2)transform.position);

        this.AttackPlayer();
    }

    private void RaiseTheDead()
    {
        this.MovementState = MovementState.Idle;
        this.isStopMove = true;

        int totalDeath = this.enemyCtrl.BattleZone.DeathZone.Count;
        int amount = this.isSummonAll ? totalDeath : (totalDeath < this.amountSummon ? totalDeath : this.amountSummon);
        this.checkLoop2 = 0;

        for (int i = 0; i < amount; i++)
        {
            while (!this.enemyCtrl.BattleZone.DeathZone[i].gameObject.activeSelf)
            {
                i++;
                amount++;
                this.checkLoop2++;

                if (this.checkLoop2 > 100)
                {
                    Debug.LogWarning("The while loop in SummonSkull() has run too much!", transform.gameObject);
                    return;
                }
            }

            StartCoroutine(this.GetSkeleton(this.enemyCtrl.BattleZone.DeathZone[i].gameObject));
        }
    }

    private IEnumerator GetSkeleton(GameObject corpseObj)
    {
        Instantiate(this.animationRaiseDeadPrefab, (Vector2)corpseObj.transform.position, Quaternion.identity, this.enemyCtrl.NeverFlip.transform);
        yield return new WaitForSeconds(0.8f);

        this.isStopMove = false;
        corpseObj.SetActive(false);
        GameObject newSkeleton = Instantiate(this.skeletonPrefab, (Vector2)corpseObj.transform.position, Quaternion.identity);
        newSkeleton.transform.localScale = corpseObj.transform.localScale;
        newSkeleton.GetComponent<EnemyCtrl>().EnemyAnimator.Play("6_Raise");
        this.enemyCtrl.BattleZone.AddEnemyToRoom(newSkeleton);
    }

    private void FinishUseSkill() //Call in Invoke
    {
        this.isUsingSkill = false;
    }
}
