using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyLife enemyLife;

    public EnemyAI EnemyAI => this.enemyAI;
    public EnemyLife EnemyLife => this.enemyLife;

    private void Awake()
    {
        this.enemyAI = GetComponentInChildren<EnemyAI>();
        this.enemyLife = GetComponentInChildren<EnemyLife>();
    }
}
