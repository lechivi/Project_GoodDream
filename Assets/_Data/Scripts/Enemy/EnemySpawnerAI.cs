using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerAI : EnemyAI
{
    [Header("ENEMY SPAWNER")]
    [SerializeField] protected GameObject animationSummonPrefab;
    [SerializeField] protected GameObject animationSummonerPrefab;
    [SerializeField] protected List<GameObject> listSpawnEnemy;
    [SerializeField] protected int amountSpawn;
    [SerializeField] protected float rangeSpawn;
    [SerializeField] protected bool isRandomAmountSpawn;

    protected bool checkHealth1;
    protected bool checkHealth2;
    protected bool isUsingSkill;

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
            if (!this.enemyCtrl.BattleZone.IsPlayerEnter || this.isUsingSkill) return;
            this.ActionWhenDetectPlayer();
        }

        EnemyLife enemyLife = this.enemyCtrl.EnemyLife;
        if (enemyLife.Health <= enemyLife.MaxHealth * 2/3 && enemyLife.Health > enemyLife.MaxHealth * 1/4 && !this.checkHealth1)
        {
            this.isUsingSkill = true;
            this.checkHealth1 = true;
            this.isRandomAmountSpawn = false;
            this.SpawnEnemy();
            Invoke("FinishUseSkill", 1f);
        }
        else if (enemyLife.Health <= enemyLife.MaxHealth * 1/4 && !this.checkHealth2)
        {
            this.isUsingSkill = true;
            this.checkHealth2 = true;
            this.isRandomAmountSpawn = true;
            this.SpawnEnemy();
            Invoke("FinishUseSkill", 1f);
        }
    }

    protected override void ActionWhenDetectPlayer()
    {
        base.ActionWhenDetectPlayer();
        this.Facing(this.enemyPlayerDetector.Player.position);

        if (Vector2.Distance(transform.position, this.enemyPlayerDetector.Player.position) > this.distanceAttack) //Compare distance-from-player with distanceAttack
        {
            this.MoveToTarget(this.enemyPlayerDetector.Player.position, Random.Range(this.moveSpeed - 2, this.moveSpeed + 2));
            this.WeaponRotation(new Vector2(this.enemyCtrl.transform.localScale.x == this.originScale.x ? this.directionWeapon : -this.directionWeapon, 1f));
        }
        else
        {
            this.AttackPlayer();
        }
    }

    protected virtual void SpawnEnemy()
    {
        this.MovementState = MovementState.Idle;
        this.isStopMove = true;

        int amount = this.isRandomAmountSpawn ? Random.Range(this.amountSpawn, this.amountSpawn + 3) : this.amountSpawn;
        Instantiate(this.animationSummonerPrefab, (Vector2) transform.position + new Vector2(0, 0.3f), Quaternion.identity, this.enemyCtrl.NeverFlip.transform);

        for (int i =0; i < amount; i++)
        {
            StartCoroutine(GetRandomEnemy());
        }
       
    }

    protected virtual IEnumerator GetRandomEnemy()
    {
        yield return new WaitForSeconds(0.5f);

        Vector2 spawnPosition;
        do
        {
            this.checkLoop += 1;
            if (this.checkLoop >= 100)
            {
                Debug.Log("break");
                spawnPosition = this.transform.position;
                break;
            }

            spawnPosition = new Vector2(transform.position.x + Random.Range(-roamRange, roamRange), transform.position.y + Random.Range(-roamRange, roamRange));
        } while (!this.enemyCtrl.BattleZone.Col.bounds.Contains(spawnPosition));
        this.checkLoop = 0;

        Instantiate(this.animationSummonPrefab, spawnPosition + new Vector2(0, 0.3f), Quaternion.identity);
        yield return new WaitForSeconds(1f);

        this.isStopMove = false;
        int typeEnemy = Random.Range(0, this.listSpawnEnemy.Count);
        GameObject newEnemy = Instantiate(this.listSpawnEnemy[typeEnemy], spawnPosition, Quaternion.identity, this.enemyCtrl.BattleZone.transform.Find("ListEnemy"));
        EnemyCtrl newEnemyCtrl = newEnemy.GetComponent<EnemyCtrl>();

        newEnemyCtrl.BattleZone = this.enemyCtrl.BattleZone;
        this.enemyCtrl.BattleZone.EnemiesInRoom.Add(newEnemyCtrl);
    }

    private void FinishUseSkill() //Call in Invoke
    {
        this.isUsingSkill = false;
    }

}
