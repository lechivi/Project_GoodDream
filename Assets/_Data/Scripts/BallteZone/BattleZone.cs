using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Wave
{
    [SerializeField] private List<GameObject> listEnemy = new List<GameObject>();
    public List<GameObject> ListEnemy => this.listEnemy;
}

public class BattleZone : MonoBehaviour
{
    [SerializeField] private List<Wave> wave;
    [SerializeField] private List<CheckDoor> checkDoors = new List<CheckDoor>();
    [SerializeField] private GameObject animationSpawnPrefab;

    private List<EnemyCtrl> enemiesInRoom = new List<EnemyCtrl>();
    private BoxCollider2D col;
    private bool isCompleted;
    private bool isCreatingNewWave;
    private int currentWave = -1;
    private int checkLoop = 0;

    public List<EnemyCtrl> EnemiesInRoom { get => this.enemiesInRoom; set => this.enemiesInRoom = value; }
    public BoxCollider2D Col { get => this.col; }
    public bool IsPlayerEnter { get; set; } //call when the player enter room

    private void Awake()
    {
        this.col = GetComponent<BoxCollider2D>();
        this.EnemiesInRoom = new List<EnemyCtrl>();

        foreach (Transform child in transform.Find("CheckDoors"))
        {
            this.checkDoors.Add(child.GetComponent<CheckDoor>());
        }   

        //if (transform.Find("ListEnemy") != null)
        //{
        //    foreach (Transform child in transform.Find("ListEnemy"))
        //    {
        //        EnemyCtrl enemyCtrl = child.GetComponent<EnemyCtrl>();
        //        if (enemyCtrl == null) continue;

        //        enemyCtrl.BattleZone = this;
        //        this.enemiesInRoom.Add(enemyCtrl);
        //    }

        //}
       
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
                }
            }

        }

        if (this.enemiesInRoom.Count == 0 && !this.isCreatingNewWave && this.IsPlayerEnter)
        {
            this.currentWave++;
            if (this.currentWave == this.wave.Count)
            {
                foreach (CheckDoor door in checkDoors)
                {
                    door.OpenDoor();
                }
                this.isCompleted = true;
                this.IsPlayerEnter = false;
            }
            else
            {
                this.isCreatingNewWave = true;
                StartCoroutine(this.StartNewWave());
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && collision.gameObject.CompareTag("ColliderWithWall") && !this.isCompleted)
        {
            this.IsPlayerEnter = true;

            foreach (CheckDoor door in checkDoors)
            {
                door.CloseDoor();
            }
        }
    }

    private IEnumerator StartNewWave()
    {
        this.enemiesInRoom.Clear();
        Debug.Log($"Wave {this.currentWave + 1}/{this.wave.Count}");
        yield return new WaitForSeconds(1f);

        foreach (GameObject enemyObj in this.wave[this.currentWave].ListEnemy)
        {
            StartCoroutine(this.SpawnNewEnemy(enemyObj));
        }
    }

    private IEnumerator SpawnNewEnemy(GameObject enemyObj)
    {
        Vector2 spawnPosition;
        Vector2 randomPoint;

        do
        {
            this.checkLoop += 1;
            if (this.checkLoop >= 100)
            {
                Debug.Log("break");
                spawnPosition = this.transform.position;
                break;
            }
            randomPoint = Random.insideUnitCircle * (this.col.size / 2);
            spawnPosition = (Vector2)transform.position + randomPoint;
        } while (!this.col.bounds.Contains(spawnPosition));
        this.checkLoop = 0;

        Instantiate(this.animationSpawnPrefab, spawnPosition + new Vector2(0, 0.3f), Quaternion.identity, transform.Find("ListEnemy"));
        yield return new WaitForSeconds(1f);

        GameObject newEnemy = Instantiate(enemyObj, spawnPosition, Quaternion.identity, transform.Find("ListEnemy"));
        EnemyCtrl enemyCtrl = newEnemy.GetComponent<EnemyCtrl>();

        if (enemyCtrl != null)
        {
            enemyCtrl.BattleZone = this;
            this.enemiesInRoom.Add(enemyCtrl);
            this.isCreatingNewWave = false;
        }
    }

}
