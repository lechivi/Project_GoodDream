using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private TabGroup tabGroup;

    private void OnEnable()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveSettingPanel(true);
            UIManager.Instance.SettingPanel.SetupValueOG();
            UIManager.Instance.ActiveSettingPanel(false);
        }
    }

    public void FullAllTab()
    {
        foreach(var tabButton in this.tabGroup.tabButtons)
        {
            tabButton.gameObject.SetActive(true);
        }

        foreach(var obj in this.tabGroup.objectToSwap)
        {
            obj.gameObject.SetActive(true);
        }
    }

    public void OnClickedResumeButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }

        if (GameManager.HasInstance && UIManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
            UIManager.Instance.ActivePausePanel(false);
        }
    }

    public void OnClickedSettingButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveSettingPanel(true);
            UIManager.Instance.ActivePausePanel(false);
        }
    }

    public void OnClickedMainMenuButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.BackToMainMenu();
        }
    }

    public void OnClickedQuitButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.QuitGame();
        }
    }
}
