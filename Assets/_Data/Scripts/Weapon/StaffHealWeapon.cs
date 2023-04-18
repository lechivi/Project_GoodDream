using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffHealWeapon : WeaponMagic
{
    [Header("Staff Heal")]
    [SerializeField] private GameObject healPrefab;
    [SerializeField] private int healAmount;

    protected override void SpellMove()
    {
        this.HealSpell();
        Debug.Log("Spell");

    }

    private void HealSpell()
    {
        PlayerLife playerLife = this.weaponParent.PlayerCtrl.PlayerLife;
        if (playerLife.Health < playerLife.MaxHealth)
        {
            this.isReadySpellMove = false;
            this.isStartCooldownSpellMove = true;

            this.animator.SetTrigger("Magic");
            this.weaponParent.PlayerCtrl.PlayerMagic.UseMana(this.manaCostSpell);
            playerLife.Heal(this.healAmount);
            Instantiate(healPrefab, transform.position, Quaternion.identity, this.weaponParent.PlayerCtrl.NeverFlip.Target);
        }
    }

}
