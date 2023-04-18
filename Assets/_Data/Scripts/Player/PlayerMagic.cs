using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagic : PlayerAbstract
{
    public delegate void PlayerMana(int manaCost, bool isMagic);
    public static PlayerMana playerManaDelegate;

    [SerializeField] private int mana;
    [SerializeField] private int maxMana;

    public int Mana { get => this.mana; set => this.mana = value; }
    public int MaxMana { get => this.maxMana; set => this.maxMana = value; }

    private void Start()
    {
        if (GameManager.HasInstance)
        {
            this.mana = GameManager.Instance.CurrentMana;
            this.maxMana = GameManager.Instance.MaxMana;
        }
    }

    public void UseMana(int costMana)
    {
        if (this.mana < 0) return;

        this.mana -= costMana;
        if (this.mana <= 0)
        {
            this.mana = 0;
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.UpdateMana(this.mana);
            playerManaDelegate(this.mana, true);
        }
    }

    public void RestoreMana(int amount)
    {
        this.mana += amount;
        if (this.mana > this.maxMana)
            this.mana = this.maxMana;

        if (GameManager.HasInstance)
        {
            GameManager.Instance.UpdateMana(this.mana);
            playerManaDelegate(this.mana, true);
        }
    }
}
