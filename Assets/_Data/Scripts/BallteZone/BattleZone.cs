using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleZone : MonoBehaviour
{
    [SerializeField] private List<CheckDoor> checkDoors = new List<CheckDoor>();
    [SerializeField] private List<EnemyCtrl> enemies = new List<EnemyCtrl>();
    public bool CompletedZone { get; set; }

    private int currentEnemy = 0;

    private void Awake()
    {
        foreach (Transform child in transform.Find("CheckDoors"))
        {
            this.checkDoors.Add(child.GetComponent<CheckDoor>());
        }   

        if (transform.Find("ListEnemy") != null)
        {
            foreach (Transform child in transform.Find("ListEnemy"))
            {
                this.enemies.Add(child.GetComponent<EnemyCtrl>());
            }

            this.currentEnemy = this.enemies.Count;
        }
       
    }

    private void Update()
    {
        if (this.enemies.Count > 0)
        {
            foreach (EnemyCtrl child in enemies)
            {
                if (child.EnemyLife.enemyDeath)
                {
                    this.currentEnemy -= 1;
                }
            }
        }

        if (this.currentEnemy == 0)
        {
            this.CompletedZone = true;
        }

        if (this.CompletedZone)
        {
            foreach (CheckDoor door in checkDoors)
            {
                door.OpenDoor();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !this.CompletedZone)
        {
            foreach (CheckDoor door in checkDoors)
            {
                door.CloseDoor();
            }
        }
    }
}
