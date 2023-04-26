using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelItemParent : MonoBehaviour
{
    public virtual bool CheckCanAddItem()
    {
        return false;
        //For overrite
    }

    public virtual void AddItem(WeaponNormalSO item) 
    {
        //For overrite
    }

    public virtual void RemoveItem(WeaponNormalSO item)
    {
        //For overrite
    }

}
