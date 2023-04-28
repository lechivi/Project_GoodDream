using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolderCtrl : MonoBehaviour
{
    public static ItemHolderCtrl instance;
    [SerializeField] private List<ItemHolderZone> zones = new List<ItemHolderZone>();

    [SerializeField] private List<ItemHolderZone> activeZones = new List<ItemHolderZone>();

    [SerializeField] private int number = 5;

    private void Awake()
    {
        ItemHolderCtrl.instance = this;

        if (this.zones.Count == 0)
        {
            foreach (Transform child in transform)
            {
                ItemHolderZone zone = child.GetComponent<ItemHolderZone>();
                if (zone != null)
                {
                    zones.Add(zone);
                }
            }
        }

    }

    public void SetActiveRandomZone()
    {
        if (this.number > this.zones.Count) return;

        this.activeZones.Clear();
        if (this.zones.Count == 0) return;
        foreach (ItemHolderZone zone in this.zones)
        {
            zone.SelectedItems.Clear();
        }

        List<ItemHolderZone> copyZones = new List<ItemHolderZone>(this.zones);

        for (int i = 0; i < this.number && copyZones.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, copyZones.Count);

            ItemHolderZone randomZone = copyZones[randomIndex];
            randomZone.SelectRandomItem();
            this.activeZones.Add(randomZone);

            copyZones.RemoveAt(randomIndex);
        }
    }
}
