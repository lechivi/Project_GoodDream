using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerLife playerLife;
    [SerializeField] private PlayerMagic playerMagic;
    [SerializeField] private WeaponParent weaponParent;
    [SerializeField] private Animator playerAnimator;

    public PlayerMovement PlayerMovement => this.playerMovement;
    public PlayerLife PlayerLife => this.playerLife;
    public PlayerMagic PlayerMagic => this.playerMagic;
    public WeaponParent WeaponParent => this.weaponParent;
    public Animator PlayerAnimator => this.playerAnimator;

    private void Awake()
    {
        this.playerMovement = GetComponentInChildren<PlayerMovement>();
        this.playerLife = GetComponentInChildren<PlayerLife>();
        this.playerMagic = GetComponentInChildren<PlayerMagic>();
        this.weaponParent = GetComponentInChildren<WeaponParent>();
        this.playerAnimator = transform.Find("UnitRoot").GetComponent<Animator>();
    }
}
