using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerDetector : MonoBehaviour
{
    public bool PlayerInArea { get; private set; }
    public Transform Player { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBattle"))
        {
            this.PlayerInArea = true;
            this.Player = collision.GetComponentInParent<PlayerCtrl>().transform;
            //Debug.Log(Player.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBattle"))
        {
            this.PlayerInArea = false;
            this.Player = null;
        }
    }
}
