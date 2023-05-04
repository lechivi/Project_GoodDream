using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    [Header("WEAPON SETTING")]
    [SerializeField] protected WeaponSO weaponSO;
    [SerializeField] protected int minDamage = 1;
    [SerializeField] protected int maxDamage = 5;
    [SerializeField] protected int criticalChance = 10;
    [SerializeField] protected float speedAttack = 5f;
    public MagicType MagicType;

    public WeaponSO WeaponSO => this.weaponSO;
    public bool IsUsing;
    public WeaponType WeaponType { get; set; }

    protected WeaponParent weaponParent;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    protected virtual void Awake()
    {
        this.weaponParent = transform.parent.GetComponent<WeaponParent>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        //For overrite
    }

    public virtual void SetActiveWeapon(bool active)
    {
        if (active)
        {
            this.IsUsing = true;
            this.spriteRenderer.enabled = true;
        }
        else
        {
            this.IsUsing = false;
            this.spriteRenderer.enabled = false;
        }
    }

    protected virtual int GetRandomDamage()
    {
        bool isCritical = Random.Range(0, 101) <= this.criticalChance;
        //if (isCritical) damageText.text.activeColor = Color.red

        return isCritical ? this.maxDamage : Random.Range(this.minDamage, this.maxDamage);
    }

    public virtual void OnClickedFirstMoveButton()
    {
        //For overrite
        Debug.Log("A");
    }

    public virtual void OnClickedSecondMoveButton()
    {
        //For overrite
        Debug.Log("B");
    }

    public virtual void EnemyUseWeapon()
    {
        //Only enemy use this method
        //For overrite
    }

}
