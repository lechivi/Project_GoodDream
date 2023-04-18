using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : PlayerAbstract
{
    public bool CanCollectMana()
    {
        return this.playerCtrl.PlayerMagic.Mana != this.playerCtrl.PlayerMagic.MaxMana;
    } 

    public void CollectMana(int value)
    {
        this.playerCtrl.PlayerMagic.RestoreMana(value);
    }

    public bool CanCollectHealth()
    {
        return this.playerCtrl.PlayerLife.Health != this.playerCtrl.PlayerLife.MaxHealth;
    }

    public void CollectHealth(int value)
    {
        this.playerCtrl.PlayerLife.Heal(value);

    }
}
