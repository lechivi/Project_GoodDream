using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponPowerSO", menuName = "SO/WeaponPower")]
public class WeaponPowerSO : WeaponSO
{
    [Space(10)]
    public MagicType magic;
    public WeaponNormalSO originWeapon;
}
