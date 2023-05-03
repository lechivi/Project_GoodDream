using EasyMobileInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerAbstract
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Joystick movementJoystick;
    public MovementState MovementState { get; set; }
    public bool IsFacingRight;
    public bool IsUseJoystic;
    public bool CanMove = true;

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

    private void Start()
    {
        if (UIManager.HasInstance)
        {
            GuidePopup guidePopup = UIManager.Instance.GuideCtrl.GetGuide(Guide.OpenInv);
            if (guidePopup != null && !guidePopup.First)
            {
                this.CanMove = false;
                guidePopup.First = true;
                UIManager.Instance.GuideCtrl.SetTrueGuide(guidePopup);
            }
        }
    }

    private void Update()
    {
        if (this.rb.bodyType == RigidbodyType2D.Static) return;

        this.UpdateAnimation();

        if (UIManager.HasInstance)
        {
            GuidePopup guidePopup = UIManager.Instance.GuideCtrl.GetGuide(Guide.OpenInv);
            if (guidePopup.Show)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    this.CanMove = true;
                    UIManager.Instance.GuideCtrl.SetFalseGuide();
                    UIManager.Instance.HomeScenePanel.TimerRemainCtrl.RunTime();
                }
                this.movement = Vector2.zero;
                this.MovementState = MovementState.Idle;
                return;
            }
        }

        if (!this.CanMove) return;

        if (this.MovementState == MovementState.Death) return;

        this.movement.x = Input.GetAxisRaw("Horizontal");
        this.movement.y = Input.GetAxisRaw("Vertical");

        this.MovementState = this.movement == Vector2.zero ? MovementState.Idle : MovementState.Run;
        //if (!this.IsUseJoystic)
        //{
        //    this.movement.x = Input.GetAxisRaw("Horizontal");
        //    this.movement.y = Input.GetAxisRaw("Vertical");

        //    this.MovementState = this.movement == Vector2.zero ? MovementState.Idle : MovementState.Run;
        //}
        //else
        //{
        //    this.MovementState = this.movementJoystick.CurrentProcessedValue == Vector3.zero ? MovementState.Idle : MovementState.Run;
        //}

        this.Facing();
    }

    private void FixedUpdate()
    {
        if (this.rb.bodyType == RigidbodyType2D.Static) return;

        this.rb.MovePosition(this.rb.position + this.movement.normalized * this.moveSpeed * Time.fixedDeltaTime);
        //if (!this.IsUseJoystic)
        //{
        //    this.rb.MovePosition(this.rb.position + this.movement.normalized * this.moveSpeed * Time.fixedDeltaTime);
        //}
        //else
        //{
        //    this.playerCtrl.transform.position += this.movementJoystick.CurrentProcessedValue * this.moveSpeed * Time.fixedDeltaTime;
        //}
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
        //if (!this.IsUseJoystic)
        //{
        //    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    if (mousePos.x > this.playerCtrl.transform.position.x)
        //    {
        //        this.playerCtrl.transform.localScale = new Vector3(-1, 1, 1);
        //        this.IsFacingRight = true;
        //    }
        //    else
        //    {
        //        this.playerCtrl.transform.localScale = Vector3.one;
        //        this.IsFacingRight = false;
        //    }
        //}
        //else
        //{
        //    if (this.movementJoystick.CurrentProcessedValue.x > 0)
        //    {
        //        this.playerCtrl.transform.localScale = new Vector3(-1, 1, 1);
        //        this.IsFacingRight = true;
        //    }
        //    else if(this.movementJoystick.CurrentProcessedValue.x < 0)
        //    {
        //        this.playerCtrl.transform.localScale = Vector3.one;
        //        this.IsFacingRight = false;
        //    }
        //}

    }

    private void UpdateAnimation()
    {
        if (this.MovementState == MovementState.Idle)
        {
            this.playerCtrl.PlayerAnimator.Play("0_idle");
        }
        else if (this.MovementState == MovementState.Run)
        {
            this.playerCtrl.PlayerAnimator.Play("1_Run");
        }
        else if (this.MovementState == MovementState.Death)
        {
            this.playerCtrl.PlayerAnimator.Play("4_Death");
        }
    }    
}
