using EasyMobileInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerAbstract
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Joystick movementJoystick;
    public MovementState movementState { get; set; }
    public bool IsFacingRight;

    private Rigidbody2D rb;
    private Vector2 movement;

    protected override void Awake()
    {
        base.Awake();
        this.rb = this.playerCtrl.GetComponent<Rigidbody2D>();

        if (UIManager.HasInstance)
        {
            if (UIManager.Instance.GamePanel.MovementJoystick == null) return;
            this.movementJoystick = UIManager.Instance.GamePanel.MovementJoystick;
        }
    }

    private void Update()
    {
        this.UpdateAnimation();
        if (this.movementState == MovementState.Death) return;

        if (this.movementJoystick == null)
        {
            this.movement.x = Input.GetAxisRaw("Horizontal");
            this.movement.y = Input.GetAxisRaw("Vertical");

            this.movementState = this.movement == Vector2.zero ? MovementState.Idle : MovementState.Run;
        }
        else
        {
            this.movementState = this.movementJoystick.CurrentProcessedValue == Vector3.zero ? MovementState.Idle : MovementState.Run;
        }

        this.Facing();
    }

    private void FixedUpdate()
    {
        if (this.movementJoystick == null)
        {
            this.rb.MovePosition(this.rb.position + this.movement.normalized * this.moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            this.playerCtrl.transform.position += this.movementJoystick.CurrentProcessedValue * this.moveSpeed * Time.fixedDeltaTime;
        }
        //Debug.Log(this.movementJoystick.CurrentProcessedValue);
    }

    private void Facing()
    {
        if (this.movementJoystick == null)
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
        else
        {
            if (this.movementJoystick.CurrentProcessedValue.x > 0)
            {
                this.playerCtrl.transform.localScale = new Vector3(-1, 1, 1);
                this.IsFacingRight = true;
            }
            else if(this.movementJoystick.CurrentProcessedValue.x < 0)
            {
                this.playerCtrl.transform.localScale = Vector3.one;
                this.IsFacingRight = false;
            }
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
