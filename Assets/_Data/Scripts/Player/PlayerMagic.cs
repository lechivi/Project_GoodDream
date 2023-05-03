using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagic : PlayerAbstract
{
    public delegate void PlayerMana(int manaCost, int MaxMana);
    public static PlayerMana playerManaDelegate;

    [SerializeField] private int mana;
    [SerializeField] private int maxMana;

    public int Mana { get => this.mana; set => this.mana = value; }
    public int MaxMana { get => this.maxMana; set => this.maxMana = value; }

    private void Start()
    {
        if (PlayerManager.HasInstance)
        {
            this.mana = PlayerManager.Instance.CurrentMana;
            this.maxMana = PlayerManager.Instance.MaxMana;
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

        if (PlayerManager.HasInstance)
        {
            PlayerManager.Instance.UpdateMana(this.mana);
            playerManaDelegate(this.mana, this.maxMana);
        }
    }

    public void RestoreMana(int amount)
    {
        this.mana += amount;
        if (this.mana > this.maxMana)
            this.mana = this.maxMana;

        if (PlayerManager.HasInstance)
        {
            PlayerManager.Instance.UpdateMana(this.mana);
            playerManaDelegate(this.mana, this.maxMana);
        }
    }
}
