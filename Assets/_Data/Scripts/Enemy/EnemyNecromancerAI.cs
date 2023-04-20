using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNecromancerAI : EnemyAI
{
    [Header("NECROMANCER")]
    [SerializeField] private GameObject skeletonPrefab;
    [SerializeField] private GameObject animationRaiseDeadPrefab;
    [SerializeField] private int amountSummon;

    private bool isSummonAll;
    private int checkLoop2;

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            RaiseTheDead();
        }
    }

    private void RaiseTheDead()
    {
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
        Instantiate(this.animationRaiseDeadPrefab, (Vector2)corpseObj.transform.position + new Vector2(0, 0.3f), Quaternion.identity, this.enemyCtrl.NeverFlip.transform);
        yield return new WaitForSeconds(0.85f);

        corpseObj.SetActive(false);
        GameObject newSkeleton = Instantiate(this.skeletonPrefab, (Vector2)corpseObj.transform.position, Quaternion.identity);
        this.enemyCtrl.BattleZone.AddEnemyToRoom(newSkeleton);
        enemyCtrl.EnemyAnimator.Play("6_Raise");
    }
}
