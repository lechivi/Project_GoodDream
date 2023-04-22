using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponNormalSO", menuName = "SO/WeaponNormal")]
public class WeaponNormalSO : ScriptableObject
{
    public string weaponName;
    public int minDamage;
    public int maxDamage;
    public Sprite image;
    public WeaponType type;
    public float rateEvo;
    public float rateMagicFire;
    public float rateMagicLightning;
    public float rateMagicPoison;
    public float rateMagicIce;
    public List<WeaponPowerSO> evolutionWeapon;
}
