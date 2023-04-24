using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponNormalSO", menuName = "SO/WeaponNormal")]
public class WeaponNormalSO : WeaponSO
{
    [Space(10)]
    public float rateEvo;
    public float rateMagicFire;
    public float rateMagicLightning;
    public float rateMagicPoison;
    public float rateMagicIce;
    public List<WeaponPowerSO> evolutionWeapon;
}
