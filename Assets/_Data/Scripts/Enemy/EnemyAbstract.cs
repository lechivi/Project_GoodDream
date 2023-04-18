using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAbstract : MonoBehaviour
{
    protected EnemyCtrl enemyCtrl;
    public EnemyCtrl EnemyCtrl { get => this.enemyCtrl; }
    
    protected virtual void Awake()
    {
        this.enemyCtrl = transform.parent.GetComponent<EnemyCtrl>();
    }
}
