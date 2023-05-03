using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookWeapon : WeaponMagic
{

    protected override void AttackMove()
    {
        base.AttackMove();
        this.isReadyAttackMove = false;
        this.isStartCooldownAttackMove = true;

        this.ShootBulletSpell(false);
    }
}
