using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int min = 0;
    public int max = 3;
    public int criticalChance = 3;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            int chance = Random.Range(0, 101);
            bool isCritical = chance <= this.criticalChance;
            Debug.Log(isCritical ? "Critical (" + chance + "): " + this.max : Random.Range(this.min, this.max));
        }
    }
}
