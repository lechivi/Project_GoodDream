using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleZone : MonoBehaviour
{
    [SerializeField] private List<CheckDoor> checkDoors = new List<CheckDoor>();
    [SerializeField] private List<EnemyCtrl> enemiesInRoom = new List<EnemyCtrl>();

    public List<EnemyCtrl> EnemiesInRoom { get => this.enemiesInRoom; set => this.enemiesInRoom = value; }
    public BoxCollider2D Col { get; private set; }
    public bool IsPlayerEnter { get; set; } //call when the player enter room

    private int currentEnemy = 0;

    private void Awake()
    {
        this.Col = GetComponent<BoxCollider2D>();
        this.EnemiesInRoom = new List<EnemyCtrl>();

        foreach (Transform child in transform.Find("CheckDoors"))
        {
            this.checkDoors.Add(child.GetComponent<CheckDoor>());
        }   

        if (transform.Find("ListEnemy") != null)
        {
            foreach (Transform child in transform.Find("ListEnemy"))
            {
                EnemyCtrl enemyCtrl = child.GetComponent<EnemyCtrl>();
                if (enemyCtrl == null) continue;

                enemyCtrl.BattleZone = this;
                this.enemiesInRoom.Add(enemyCtrl);
            }

            this.currentEnemy = this.enemiesInRoom.Count;
        }
       
    }

    private void Update()
    {
        if (this.enemiesInRoom.Count > 0)
        {
            for (int i = 0; i < this.enemiesInRoom.Count; i++)
            {
                if (this.enemiesInRoom[i].EnemyLife.Health == 0)
                {
                    this.enemiesInRoom.RemoveAt(i);
                    this.currentEnemy--;
                }
            }

        }

        if (this.currentEnemy == 0)
        {
            foreach (CheckDoor door in checkDoors)
            {
                door.OpenDoor();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && collision.gameObject.CompareTag("ColliderWithWall") && this.currentEnemy != 0)
        {
            this.IsPlayerEnter = true;

            foreach (CheckDoor door in checkDoors)
            {
                door.CloseDoor();
            }
        }

    }

}
