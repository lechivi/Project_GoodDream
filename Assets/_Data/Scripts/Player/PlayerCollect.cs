using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : PlayerAbstract
{
    public bool CanCollectMana(int value)
    {
        if (this.playerCtrl.PlayerMagic.Mana == this.playerCtrl.PlayerMagic.MaxMana)
            return false;
        else
        {
            this.playerCtrl.PlayerMagic.RestoreMana(value);
            return true;
        }
    } 
    
    public bool CanCollectHealth(int value)
    {
        if (this.playerCtrl.PlayerLife.Health == this.playerCtrl.PlayerLife.MaxHealth)
            return false;
        else
        {
            this.playerCtrl.PlayerLife.Heal(value);
            return true;
        }
    }
}
