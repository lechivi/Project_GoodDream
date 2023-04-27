using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScenePanel : MonoBehaviour
{
    [SerializeField] private PlayerBasicMovement playerBasicMovement;
    [SerializeField] private PlayerBasicHolder playerBasicHolder;
    [SerializeField] private ItemHolderCtrl itemHolderCtrl;
    [SerializeField] private PanelItemCtrl panelItemCtrl;
    [SerializeField] private PanelHandHolderCtrl panelHandHolderCtrl;
    [SerializeField] private TimerRemainCtrl timerRemainCtrl;
    [SerializeField] private Button closePanelItemButton;

    private bool check;

    private void Start()
    {
        this.playerBasicMovement.CanMove = false;
        this.panelItemCtrl.gameObject.SetActive(false);
        this.panelHandHolderCtrl.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!this.panelHandHolderCtrl.CheckCanAddItem())
        {
            if (UIManager.HasInstance)
            {
                 UIManager.Instance.NotificationPanel.NotificationHUD.SetNotiText("Hand is full! Find Dream Book to store items", 5f);
                if (this.panelHandHolderCtrl.CheckCanAddItem() || !this.panelItemCtrl.gameObject.activeSelf)
                {
                    UIManager.Instance.NotificationPanel.NotificationHUD.HideText();
                }
            }

        }

        if ((this.timerRemainCtrl.TimeOut || Input.GetKeyDown(KeyCode.L)) && !this.check)
        {
            this.check = true;
            this.playerBasicMovement.CanMove = false;

            if (DreamBookScript.instance.HolderItems.Count == 0  && this.playerBasicHolder.HolderItems.Count == 0)
            {
                Debug.Log("Can't find any weapon. You can't enter Dream World!");
            }
            else
            {
                this.panelHandHolderCtrl.gameObject.SetActive(false);

                if (PlayerManager.HasInstance)
                {
                    foreach (WeaponNormalSO item in DreamBookScript.instance.HolderItems)
                    {
                        PlayerManager.Instance.ListWeaponNormalSO.Add(item);
                    }

                    foreach (WeaponNormalSO item in this.playerBasicHolder.HolderItems)
                    {
                        if (item != null) PlayerManager.Instance.ListWeaponNormalSO.Add(item);
                    }

                    PlayerManager.Instance.CreateListWeapon();

                    if (UIManager.HasInstance)
                    {
                        UIManager.Instance.ActiveLoadingPanel(true);
 
                        //UIManager.Instance.ActiveGamePanel(true);
                        //if (GameManager.HasInstance)
                        //{
                        //    GameManager.Instance.StartGame();
                        //}
                    }
                }
            }
        
        }
    }
    public void OnClickedStartButton()
    {
        this.panelHandHolderCtrl.gameObject.SetActive(true);
        this.itemHolderCtrl.SetActiveRandomZone();
        this.timerRemainCtrl.RunTime();
        this.playerBasicMovement.CanMove = true;

    }
}
