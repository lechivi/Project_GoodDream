using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    [SerializeField] private BattleZone battleZone;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyLife enemyLife;
    [SerializeField] private SpawnerReward spawnerReward;
    [SerializeField] private NeverFlip neverFlip;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private Rigidbody2D rb;

    public BattleZone BattleZone {get => this.battleZone; set => this.battleZone = value; }
    public EnemyAI EnemyAI => this.enemyAI;
    public EnemyLife EnemyLife => this.enemyLife;
    public SpawnerReward SpawnerReward => this.spawnerReward;
    public NeverFlip NeverFlip => this.neverFlip;
    public Animator EnemyAnimator => this.enemyAnimator;
    public Rigidbody2D Rb => this.rb;

    private void Awake()
    {
        this.enemyAI = GetComponentInChildren<EnemyAI>();
        this.enemyLife = GetComponentInChildren<EnemyLife>();
        this.neverFlip = GetComponentInChildren<NeverFlip>();
        this.spawnerReward = GetComponentInChildren<SpawnerReward>();
        this.rb = GetComponent<Rigidbody2D>();
        this.enemyAnimator = transform.Find("UnitRoot").GetComponent<Animator>();
        this.neverFlip.Target = transform;
    }
}
