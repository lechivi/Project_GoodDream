using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavelinWeapon : WeaponShooting
{
    protected override void SetupShoot()
    {
        GetComponentInChildren<Animator>().SetTrigger("Attack");
    }

    //call Shoot() at frame animation
}
