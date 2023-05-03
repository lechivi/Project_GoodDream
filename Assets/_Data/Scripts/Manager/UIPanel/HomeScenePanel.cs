using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScenePanel : MonoBehaviour
{
    [SerializeField] private PanelItemCtrl panelItemCtrl;
    [SerializeField] private PanelHandHolderCtrl panelHandHolderCtrl;
    [SerializeField] private TimerRemainCtrl timerRemainCtrl;
    [SerializeField] private Animator animatorTransition;

    public PanelItemCtrl PanelItemCtrl => this.panelItemCtrl;
    public PanelHandHolderCtrl PanelHandHolderCtrl => this.panelHandHolderCtrl;
    public TimerRemainCtrl TimerRemainCtrl => this.timerRemainCtrl;

    private bool check;

    private void Awake()
    {
    }

    private void Start()
    {
        
        this.panelItemCtrl.gameObject.SetActive(false);
        this.panelHandHolderCtrl.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!this.panelHandHolderCtrl.CheckCanAddItem())
        {
            if (UIManager.HasInstance)
            {
                GuidePopup guidePopup = UIManager.Instance.GuideCtrl.GetGuide(Guide.FullHand);
                if (guidePopup != null && !guidePopup.First)
                {
                    guidePopup.First = true;
                    UIManager.Instance.GuideCtrl.SetTrueGuide(guidePopup);
                    UIManager.Instance.HomeScenePanel.TimerRemainCtrl.PauseTime();
                }
            }

        }

        if (UIManager.HasInstance)
        {
            GuidePopup guidePopupDragToHand = UIManager.Instance.GuideCtrl.GetGuide(Guide.DragToHand);
            GuidePopup guidePopupFullHand = UIManager.Instance.GuideCtrl.GetGuide(Guide.FullHand);
            if (guidePopupDragToHand.Show || guidePopupFullHand.Show)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    UIManager.Instance.GuideCtrl.SetFalseGuide();
                }
            }
        }
            

        if ((this.timerRemainCtrl.TimeOut || Input.GetKeyDown(KeyCode.L)) && !this.check)
        {
            this.check = true;
            PlayerBasicCtrl.instance.PlayerMovement.CanMove = false;

            if (DreamBookScript.instance.HolderItems.Count == 0  && PlayerBasicCtrl.instance.PlayerHolder.HolderItems.Count == 0)
            {
                Debug.Log("Can't find any weapon. You can't enter Dream World!");
            }
            else
            {
                this.panelHandHolderCtrl.gameObject.SetActive(false);

                if (!PlayerManager.HasInstance) return;

                foreach (WeaponNormalSO item in DreamBookScript.instance.HolderItems)
                {
                    PlayerManager.Instance.ListWeaponNormalSO.Add(item);
                }

                foreach (WeaponNormalSO item in PlayerBasicCtrl.instance.PlayerHolder.HolderItems)
                {
                    if (item != null) PlayerManager.Instance.ListWeaponNormalSO.Add(item);
                }

                PlayerManager.Instance.CreateListWeapon();

                StartCoroutine(this.PlayButton());
            }
        }
    }

    private IEnumerator PlayButton()
    {
        this.animatorTransition = GameObject.Find("LevelLoader").GetComponentInChildren<Animator>();
        this.animatorTransition.Play("Crossfade_Start");

        yield return new WaitForSeconds(1.5f);
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveLoadingPanel(true);
        }
    }

    public void OnClickedPauseButton()
    {
        if (GameManager.HasInstance && UIManager.HasInstance)
        {
            GameManager.Instance.PauseGame();
            UIManager.Instance.ActiveSettingPanel(true);
            UIManager.Instance.SettingPanel.SettingMainMenu(false);
            UIManager.Instance.ActivePausePanel(true);
        }
    }

    public void OnClickedStartButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }

        this.panelHandHolderCtrl.gameObject.SetActive(true);
        this.timerRemainCtrl.RunTime();

        ItemHolderCtrl.instance.SetActiveRandomZone();
        PlayerBasicCtrl.instance.PlayerMovement.CanMove = true;

    }

    public void OnClickedClosePanelItem()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }

        this.panelItemCtrl.gameObject.SetActive(false);
        this.timerRemainCtrl.RunTime();
    }
}
