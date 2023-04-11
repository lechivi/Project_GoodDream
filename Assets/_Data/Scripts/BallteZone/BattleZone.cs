using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleZone : MonoBehaviour
{
    [SerializeField] private List<CheckDoor> checkDoors = new List<CheckDoor>();
    public bool CompletedZone { get; set; }

    private void Awake()
    {
        foreach (Transform child in transform.Find("CheckDoors"))
        {
            this.checkDoors.Add(child.GetComponent<CheckDoor>());
        }
    }

    private void Update()
    {
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
