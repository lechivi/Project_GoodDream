using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerAbstract
{
    [SerializeField] private float moveSpeed = 5f;

    public MovementState movementState { get; set; }
    public bool IsFacingRight;

    private Rigidbody2D rb;
    private Vector2 movement;

    protected override void Awake()
    {
        base.Awake();
        this.rb = this.playerCtrl.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        this.UpdateAnimation();
        if (this.movementState == MovementState.Death) return;

        this.movement.x = Input.GetAxisRaw("Horizontal");
        this.movement.y = Input.GetAxisRaw("Vertical");

        this.movementState = this.movement == Vector2.zero ? MovementState.Idle : MovementState.Run;

        this.Facing();
    }

    private void FixedUpdate()
    {
        this.rb.MovePosition(this.rb.position + this.movement.normalized * this.moveSpeed * Time.fixedDeltaTime);
    }

    private void Facing()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x > this.playerCtrl.transform.position.x)
        {
            this.playerCtrl.transform.localScale = new Vector3(-1, 1, 1);
            this.IsFacingRight = true;
        }
        else
        {
            this.playerCtrl.transform.localScale = Vector3.one;
            this.IsFacingRight = false;
        }
    }

    private void UpdateAnimation()
    {
        if (this.movementState == MovementState.Idle)
        {
            this.playerCtrl.PlayerAnimator.Play("0_idle");
        }
        else if (this.movementState == MovementState.Run)
        {
            this.playerCtrl.PlayerAnimator.Play("1_Run");
        }
        else if (this.movementState == MovementState.Death)
        {
            this.playerCtrl.PlayerAnimator.Play("4_Death");
        }
    }    
}
