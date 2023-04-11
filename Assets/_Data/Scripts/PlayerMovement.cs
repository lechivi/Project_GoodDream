using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [SerializeField] private float moveSpeed = 5f;

    private Vector2 movement;

    public bool IsFacingRight;

    private void Update()
    {
        this.movement.x = Input.GetAxisRaw("Horizontal");
        this.movement.y = Input.GetAxisRaw("Vertical");

        this.Facing();
        this.UpdateAnimation();
    }

    private void FixedUpdate()
    {
        this.rb.MovePosition(this.rb.position + this.movement.normalized * this.moveSpeed * Time.fixedDeltaTime);
    }

    private void Facing()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x > this.transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            this.IsFacingRight = true;
        }
        else
        {
            transform.localScale = Vector3.one;
            this.IsFacingRight = false;
        }
    }

    private void UpdateAnimation()
    {
        if (this.movement == Vector2.zero)
        {
            this.animator.Play("0_idle");
        }
        else
        {
            this.animator.Play("1_Run");
        }
    }    
}
