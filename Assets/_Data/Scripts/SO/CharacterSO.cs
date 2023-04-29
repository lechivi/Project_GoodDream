using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "SO/Character")]
public class CharacterSO : ScriptableObject
{
    public string characterName;
    public int maxHealth;
    public int maxMana;
    public float reloadSpeed;
    [TextArea(2, 5)] public string description;
    public GameObject model;
}
