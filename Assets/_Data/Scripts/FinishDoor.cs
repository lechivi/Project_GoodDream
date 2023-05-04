using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishDoor : MonoBehaviour
{
    [SerializeField] private Animator animatorDoorLeft;
    [SerializeField] private Animator animatorDoorRight;
    [SerializeField] private BoxCollider2D colDoor;

    private bool isEnter;
    private bool isEnterFirstTime;

    private void Update()
    {
        if (this.isEnter)
        {
            this.isEnter = false;
            this.animatorDoorLeft.SetTrigger("Open");
            this.animatorDoorRight.SetTrigger("Open");
            Invoke("TriggerColDoor", 1f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBattle") && !this.isEnterFirstTime)
        {
            this.isEnter = true;
            this.isEnterFirstTime = true;
        }
    }

    private void TriggerColDoor()
    {
        this.colDoor.isTrigger = true;
    }
}
