using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    [Header("WEAPON SETTING")]
    [SerializeField] protected int minDamage = 1;
    [SerializeField] protected int maxDamage = 5;
    [SerializeField] protected int criticalChance = 10;
    [SerializeField] protected float speedAttack = 5f;

    public WeaponType WeaponType { get; set; }
    public bool IsUsing { get; set; }

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
        //for override
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
        //if (isCritical) damageText.text.color = Color.red

        return isCritical ? this.maxDamage : Random.Range(this.minDamage, this.maxDamage);
    }

    public virtual void EnemyUseWeapon()
    {
        //Only enemy use this method
        //for overrite
    }

}
