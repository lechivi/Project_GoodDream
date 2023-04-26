using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolderZone : MonoBehaviour
{
    [SerializeField] private PanelItemCtrl panelItemCtrl;

    public List<WeaponNormalSO> ListItem = new List<WeaponNormalSO>();
    public List<WeaponNormalSO> SelectedItems = new List<WeaponNormalSO>();

    public void SelectRandomItem()
    {
        if (this.ListItem.Count == 0) return;

        int randomAmount = Random.Range(0, this.ListItem.Count + 1);

        List<WeaponNormalSO> copyList = new List<WeaponNormalSO>(ListItem);

        for (int i = 0; i < randomAmount && copyList.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, copyList.Count);

            WeaponNormalSO randomItem = copyList[randomIndex];

            SelectedItems.Add(randomItem);

            copyList.RemoveAt(randomIndex);
        }
    }

    public void SetActivePanelItemCtrl()
    {
        this.panelItemCtrl.gameObject.SetActive(true);
        this.panelItemCtrl.SetItemFromZone(this.SelectedItems);
        this.panelItemCtrl.Zone = this;
    }
}
