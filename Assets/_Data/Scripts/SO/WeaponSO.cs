using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public int minDamage;
    public int maxDamage;
    public Sprite image;
    public WeaponType type;
    public WeaponTier tier;
}
