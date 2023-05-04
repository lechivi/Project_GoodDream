using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAllyDetector : MonoBehaviour
{
    public List<EnemyLife> Ally;
    public EnemyLife LowestHealthAlly { get; private set; }

    private void Awake()
    {
        this.Ally = new List<EnemyLife>();
    }

    public bool CheckLowestHealthAlly()
    {
        this.LowestHealthAlly = transform.parent.GetComponent<EnemyAI>().EnemyCtrl.EnemyLife;

        if (this.Ally.Count > 0 && !this.Ally.Contains(null))
        {
            foreach (EnemyLife enemyLife in this.Ally)
            {
                if (enemyLife.Health > 0 && (enemyLife.MaxHealth - enemyLife.Health > this.LowestHealthAlly.MaxHealth - this.LowestHealthAlly.Health))
                    this.LowestHealthAlly = enemyLife;
            }
        }

        if (this.LowestHealthAlly == transform.parent.GetComponent<EnemyAI>().EnemyCtrl.EnemyLife && this.LowestHealthAlly.MaxHealth == this.LowestHealthAlly.Health)
        {
            this.LowestHealthAlly = null;
            return false;
        }

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("OnTrigger", collision.gameObject);
        if (!this.Ally.Contains(collision.GetComponent<EnemyLife>()) && collision.gameObject.CompareTag("EnemyBattle"))
        {
            this.Ally.Add(collision.GetComponent<EnemyLife>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (this.Ally.Contains(collision.GetComponent<EnemyLife>()))
        //{
        //    this.Ally.Remove(collision.GetComponent<EnemyLife>());
        //}
    }

}
