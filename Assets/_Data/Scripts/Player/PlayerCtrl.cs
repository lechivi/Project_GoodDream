using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerLife playerLife;
    [SerializeField] private PlayerMagic playerMagic;
    [SerializeField] private WeaponParent weaponParent;
    [SerializeField] private NeverFlip neverFlip;
    [SerializeField] private Animator playerAnimator;

    public PlayerMovement PlayerMovement => this.playerMovement;
    public PlayerLife PlayerLife => this.playerLife;
    public PlayerMagic PlayerMagic => this.playerMagic;
    public WeaponParent WeaponParent => this.weaponParent;
    public NeverFlip NeverFlip => this.neverFlip;
    public Animator PlayerAnimator => this.playerAnimator;

    private void Awake()
    {
        this.playerMovement = GetComponentInChildren<PlayerMovement>();
        this.playerLife = GetComponentInChildren<PlayerLife>();
        this.playerMagic = GetComponentInChildren<PlayerMagic>();
        this.weaponParent = GetComponentInChildren<WeaponParent>();
        this.neverFlip = GetComponentInChildren<NeverFlip>();
        this.playerAnimator = transform.Find("UnitRoot").GetComponent<Animator>();

        this.neverFlip.Target = transform;
    }
}
