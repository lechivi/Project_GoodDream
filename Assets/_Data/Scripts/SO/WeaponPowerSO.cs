using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponPowerSO", menuName = "SO/WeaponPower")]
public class WeaponPowerSO : ScriptableObject
{
    public string weaponName;
    public int minDamage;
    public int maxDamage;
    public Sprite image;
    public WeaponType type;
    public MagicType magic;
    public WeaponNormalSO originWeapon;
}
