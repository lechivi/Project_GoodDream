using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RewardType { Health, Mana}

public class SpawnerReward : EnemyAbstract
{
    [SerializeField] private GameObject HealthCollectiblaPrefab;
    [SerializeField] private GameObject ManaCollectiblaPrefab;

    [SerializeField] private int rewardHealth;
    [SerializeField] private int rewardMana;
    [SerializeField] private bool isRandom;

    private Transform spawnPool;

    protected override void Awake()
    {
        base.Awake();
        this.spawnPool = GameObject.Find("SpawnPool").transform;
    }

    public void Spawn() //TODO: Pooling obj after
    {
        if (this.rewardHealth > 0)
        {
            int amount = this.isRandom ? Random.Range(this.rewardHealth - 5, this.rewardHealth + 5) : this.rewardHealth;
            if (amount <= 0) return;
            for (int i = 0; i < amount; i++)
            {
                Instantiate(this.HealthCollectiblaPrefab, transform.position, Quaternion.identity, this.spawnPool);
            }
        }
        if (this.rewardMana > 0)
        {
            int amount = this.isRandom ? Random.Range(this.rewardMana - 5, this.rewardMana + 5) : this.rewardMana;
            if (amount <= 0) return;
            for (int i = 0; i < amount; i++)
            {
                Instantiate(this.ManaCollectiblaPrefab, transform.position, Quaternion.identity, this.spawnPool);
            }
        }
    }
}
