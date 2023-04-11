using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    [SerializeField] private int HP = 100;

    public bool enemyDeath;

    public void TakeDamage(int damage)
    {
        this.HP -= damage;
        if (this.HP <= 0)
        {
            this.HP = 0;
            this.enemyDeath = true;
        }
    }
}
