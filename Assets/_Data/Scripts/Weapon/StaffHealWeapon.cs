using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffHealWeapon : WeaponMagic
{
    [Header("Staff Heal")]
    [SerializeField] private GameObject healPrefab;
    [SerializeField] private int healAmount;

    protected override void AttackMove()
    {
        this.isReadyAttackMove = false;
        this.isStartCooldownAttackMove = true;

        this.animator.SetTrigger("Attack");

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_MAGIC);
        }
    }

    public void ShootFrameAnimation() //Call at a shoot Frame in animation
    {
        this.ShootBulletSpell(false);
    }

    protected override void SpellMove()
    {
        this.HealSpell();
    }

    private void HealSpell()
    {
        PlayerLife playerLife = this.weaponParent.PlayerCtrl.PlayerLife;
        if (playerLife.Health < playerLife.MaxHealth)
        {
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySFX(AUDIO.SFX_SKILL_HEAL);
            }

            this.isReadySpellMove = false;
            this.isStartCooldownSpellMove = true;

            this.animator.SetTrigger("Magic");
            this.weaponParent.PlayerCtrl.PlayerMagic.UseMana(this.manaCostSpell);
            playerLife.Heal(this.healAmount);
            Instantiate(healPrefab, transform.position, Quaternion.identity, this.weaponParent.PlayerCtrl.NeverFlip.Target);
        }
    }

}
