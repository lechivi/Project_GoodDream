using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public static PlayerCtrl instance;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerLife playerLife;
    [SerializeField] private PlayerMagic playerMagic;
    [SerializeField] private WeaponParent weaponParent;
    [SerializeField] private NeverFlip neverFlip;
    [SerializeField] private PlayerModel playerModel;
    [SerializeField] private Animator playerAnimator;

    public PlayerMovement PlayerMovement => this.playerMovement;
    public PlayerLife PlayerLife => this.playerLife;
    public PlayerMagic PlayerMagic => this.playerMagic;
    public PlayerModel PlayerModel => this.playerModel;
    public WeaponParent WeaponParent => this.weaponParent;
    public NeverFlip NeverFlip => this.neverFlip;
    public Animator PlayerAnimator { get => this.playerAnimator; set => this.playerAnimator = value; }

    private void Awake()
    {
        PlayerCtrl.instance = this;

        this.playerMovement = GetComponentInChildren<PlayerMovement>();
        this.playerLife = GetComponentInChildren<PlayerLife>();
        this.playerMagic = GetComponentInChildren<PlayerMagic>();
        this.playerModel = GetComponentInChildren<PlayerModel>();
        this.playerAnimator = this.playerModel.GetComponentInChildren<Animator>();
        this.weaponParent = GetComponentInChildren<WeaponParent>();
        this.neverFlip = GetComponentInChildren<NeverFlip>();

        this.neverFlip.Target = transform;
    }
}
