using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaDetector : MonoBehaviour
{
    [SerializeField]
    public bool PlayerInArea { get; private set; }
    public Transform Player { get; private set; }

    [SerializeField]
    private string detectionTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(detectionTag))
        {
            this.PlayerInArea = true;
            this.Player = collision.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(detectionTag))
        {
            this.PlayerInArea = false;
            this.Player = null;
        }
    }
}
