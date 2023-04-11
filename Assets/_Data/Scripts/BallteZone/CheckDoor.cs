using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDoor : MonoBehaviour
{
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteUp;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D col;

    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.col = GetComponent<BoxCollider2D>();

        this.OpenDoor();
    }

    public void CloseDoor()
    {
        this.spriteRenderer.sprite = spriteUp;
        this.col.isTrigger = false;
    }

    public void OpenDoor()
    {
        this.spriteRenderer.sprite = spriteDown;
        this.col.isTrigger = true;
    }
}
