using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator reactionAnimator;
    public MovementState MovementState { get; set; }
    public bool IsFacingRight;
    public bool CanMove = false;

    private PlayerBasicHolder playerBasicHolder;
    private Rigidbody2D rb;
    private Animator playerAnimator;
    private Vector2 movement;
    private Vector3 originScale;

    private ItemHolderZone itemHolderZone;
    private bool isEnterZoneItem;
    private bool isEnterDreamBook;

    private void Awake()
    {
        this.playerBasicHolder = GetComponent<PlayerBasicHolder>();
        this.rb = GetComponent<Rigidbody2D>();
        this.playerAnimator = transform.Find("UnitRoot").GetComponent<Animator>();

        this.originScale = transform.localScale;
        this.reactionAnimator.gameObject.SetActive(false);
    }

    private void Update()
    {
        this.UpdateAnimation();

        if (!this.CanMove)
        {
            this.movement = Vector2.zero;
            this.MovementState = MovementState.Idle;
            return;
        }

        if (UIManager.HasInstance)
        {
            GuidePopup guidePopup = UIManager.Instance.GuideCtrl.GetGuide(Guide.PressE);
            if (guidePopup.GuideShow)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    UIManager.Instance.GuideCtrl.SetFalseGuide();
                    UIManager.Instance.HomeScenePanel.TimerRemainCtrl.RunTime();
                }
                this.movement = Vector2.zero;
                this.MovementState = MovementState.Idle;
                return;
            }
        }

        if (Time.timeScale == 0f ) return;

        this.movement.x = Input.GetAxisRaw("Horizontal");
        this.movement.y = Input.GetAxisRaw("Vertical");

        this.MovementState = this.movement == Vector2.zero ? MovementState.Idle : MovementState.Run;

        this.Facing();

        if (this.isEnterZoneItem && Input.GetKeyDown(KeyCode.E))
        {
            if (this.itemHolderZone != null)
            {
                this.itemHolderZone.SetActivePanelItemCtrl();
            }
            if (UIManager.HasInstance)
            {
                UIManager.Instance.HomeScenePanel.TimerRemainCtrl.PauseTime();
            }
        }

        if (this.isEnterDreamBook && Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < 2; i++)
            {
                this.playerBasicHolder.TransferItem(i);
            }
        }

        if (this.itemHolderZone != null && this.itemHolderZone.SelectedItems.Count == 0)
        {
            this.reactionAnimator.gameObject.SetActive(false);
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
            this.reactionAnimator.gameObject.SetActive(true);
            this.reactionAnimator.Rebind();
            this.isEnterZoneItem = true;
            this.itemHolderZone = itemHolderZone;

            if (UIManager.HasInstance)
            {
                GuidePopup guidePopup = UIManager.Instance.GuideCtrl.GetGuide(Guide.PressE);
                if (guidePopup != null && !guidePopup.First)
                {
                    guidePopup.First = true;
                    UIManager.Instance.GuideCtrl.SetTrueGuide(guidePopup);
                    UIManager.Instance.HomeScenePanel.TimerRemainCtrl.PauseTime();
                }
            }
        }

        DreamBookScript dreamBook = collision.gameObject.GetComponent<DreamBookScript>();
        if (dreamBook != null)
        {
            this.isEnterDreamBook = true;

            //if (UIManager.HasInstance)
            //{
            //    if (playerBasicHolder != null && playerBasicHolder.HolderItems.Count == 2 && (playerBasicHolder.HolderItems[0] != null || playerBasicHolder.HolderItems[1] != null))
            //    {
            //        UIManager.Instance.NotificationPanel.NotificationHUD.SetNotiText("Press E", 5f);
            //    }
            //}
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ItemHolderZone itemHolderZone = collision.gameObject.GetComponent<ItemHolderZone>();
        if (itemHolderZone != null/* && itemHolderZone.SelectedItems.Count != 0*/)
        {
            this.reactionAnimator.gameObject.SetActive(false);
            this.isEnterZoneItem = false;
            this.itemHolderZone = null;
        }

        DreamBookScript dreamBook = collision.gameObject.GetComponent<DreamBookScript>();
        if (dreamBook != null)
        {
            this.isEnterDreamBook = false;
        }
    }
}
