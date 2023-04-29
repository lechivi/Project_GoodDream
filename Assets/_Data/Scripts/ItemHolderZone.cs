using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolderZone : MonoBehaviour
{
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
        if (UIManager.HasInstance)
        {
            PanelItemCtrl panelItemCtrl = UIManager.Instance.HomeScenePanel.PanelItemCtrl;
            panelItemCtrl.gameObject.SetActive(true);
            panelItemCtrl.SetItemFromZone(this.SelectedItems);
            panelItemCtrl.Zone = this;

            if (UIManager.HasInstance)
            {
                GuidePopup guidePopup = UIManager.Instance.GuideCtrl.GetGuide(Guide.DragToHand);
                if (guidePopup != null && !guidePopup.First)
                {
                    guidePopup.First = true;
                    UIManager.Instance.GuideCtrl.SetTrueGuide(guidePopup);
                    UIManager.Instance.HomeScenePanel.TimerRemainCtrl.PauseTime();
                }
            }
        }

    }
}
