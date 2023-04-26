using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamBookScript : MonoBehaviour
{
    public static DreamBookScript instance;

    [SerializeField] private List<WeaponNormalSO> holderItems;
    [SerializeField] private int maxSlot = 20;
    [SerializeField] private Animator animatorGFX;

    public List<WeaponNormalSO> HolderItems { get => this.holderItems; set => this.holderItems = value; }

    private void Awake()
    {
        DreamBookScript.instance = this;
    }

    public bool AddWeapon(WeaponNormalSO weapon)
    {
        if (this.holderItems.Count == this.maxSlot) return false;
        this.holderItems.Add(weapon);
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerBasicHolder playerBasicHolder = collision.gameObject.GetComponent<PlayerBasicHolder>();
        if (playerBasicHolder != null && playerBasicHolder.HolderItems.Count == 2 && (playerBasicHolder.HolderItems[0] != null || playerBasicHolder.HolderItems[1] != null))
        {
            if (this.animatorGFX.GetCurrentAnimatorStateInfo(0).IsName("CLMagic_Enter")) return;
            this.animatorGFX.SetTrigger("Enter");
        }
    }
}
