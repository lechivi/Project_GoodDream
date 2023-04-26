using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator reactionAnimator;
    public MovementState MovementState { get; set; }
    public bool IsFacingRight;

    private Rigidbody2D rb;
    private Animator playerAnimator;
    private Vector2 movement;
    private Vector3 originScale;

    private ItemHolderZone itemHolderZone;
    private bool isEnter;

    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.playerAnimator = transform.Find("UnitRoot").GetComponent<Animator>();

        this.originScale = transform.localScale;
    }

    private void Update()
    {
        this.movement.x = Input.GetAxisRaw("Horizontal");
        this.movement.y = Input.GetAxisRaw("Vertical");

        this.MovementState = this.movement == Vector2.zero ? MovementState.Idle : MovementState.Run;

        this.Facing();
        this.UpdateAnimation();

        if (this.isEnter && Input.GetKeyDown(KeyCode.E))
        {
            if (this.itemHolderZone != null)
            {
                this.itemHolderZone.SetActivePanelItemCtrl();
            }
        }
    }

    private void FixedUpdate()
    {
        this.rb.MovePosition(this.rb.position + this.movement.normalized * this.moveSpeed * Time.fixedDeltaTime);

    }

    private void Facing()
    {
        if (this.movement.x < -0.1)
        {
            transform.localScale = new Vector3(-this.originScale.x, this.originScale.y, this.originScale.z); ;
            this.IsFacingRight = true;
        }
        else if (this.movement.x > 0.1)
        {
            transform.localScale = this.originScale;
            this.IsFacingRight = false;
        }
    }
    private void UpdateAnimation()
    {
        if (this.MovementState == MovementState.Idle)
        {
            this.playerAnimator.Play("0_idle");
        }
        else if (this.MovementState == MovementState.Run)
        {
            this.playerAnimator.Play("1_Run");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemHolderZone itemHolderZone = collision.gameObject.GetComponent<ItemHolderZone>();
        if (itemHolderZone != null && itemHolderZone.SelectedItems.Count != 0)
        {
            this.reactionAnimator.SetTrigger("Detect");
            this.isEnter = true;
            this.itemHolderZone = itemHolderZone; ;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ItemHolderZone itemHolderZone = collision.gameObject.GetComponent<ItemHolderZone>();
        if (itemHolderZone != null/* && itemHolderZone.SelectedItems.Count != 0*/)
        {
            this.reactionAnimator.SetTrigger("EndWaiting");
            this.isEnter = false;
            this.itemHolderZone = null;
        }
    }
}
