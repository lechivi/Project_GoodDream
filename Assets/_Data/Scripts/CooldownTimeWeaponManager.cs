using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CooldownTimeWeapon
{
    [SerializeField] protected WeaponMelee weapon;

    private float timerPrimaryMove;
    private float timerSpecialMove;

    public void HandleUpdate()
    {
        if (this.weapon.isStartCooldownPrimaryMove)
        {
            this.CooldownPrimaryMove();
        }
        if (this.weapon.isStartCooldownSpecialMove)
        {
            this.CooldownSpecialMove();
        }
    }

    private void CooldownPrimaryMove()
    {
        this.timerPrimaryMove += Time.deltaTime;
        if (this.timerPrimaryMove < this.weapon.cooldownTimePrimaryMove) return;

        this.timerPrimaryMove = 0;
        this.weapon.isReadyPrimaryMove = true;
        this.weapon.isStartCooldownPrimaryMove = false;

    }

    private void CooldownSpecialMove()
    {
        this.timerSpecialMove += Time.deltaTime;
        if (this.timerSpecialMove < this.weapon.cooldownTimeSpecialMove) return;

        this.timerSpecialMove = 0;
        this.weapon.isReadySpecialMove = true;
        this.weapon.isStartCooldownSpecialMove = false;
    }
}

public class CooldownTimeWeaponManager : MonoBehaviour
{
    public CooldownTimeWeapon[] weapons;

    private void Update()
    {
        this.weapons[0].HandleUpdate();
    }
}
