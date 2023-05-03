using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveDataSO", menuName = "SO/SaveData")]
public class SaveDataSO : ScriptableObject
{
    [Header("PLAYER")]
    public int CurrentHealth = -1;
    public int MaxHealth = -1;    
    public int CurrentMana = -1;
    public int MaxMana = -1;    
    public float ReloadSpeed = -1;
    public CharacterSO characterSO;

    [Header("WEAPON")]
    public List<GameObject> listWeapon;
    public List<int> listHotkey;
    public int EquipWeapon = -1;

    [Header("LEVEL MAP")]
    public int CurrentScene = -1;
    public Vector2 StartPointLevel1;
    public Vector2 StartPointLevel2;
    public Vector2 StartPointLevel3;

    [Header("GAME")]
    public bool Saved;
}
