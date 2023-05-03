using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private TabGroup tabGroup;

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

    public void OnClickedMenuButton()
    {
        if (SaveLoadManager.HasInstance)
        {
            SaveLoadManager.Instance.Save();
        }

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.RestarGame();
        }
    }

    public void OnClickedQuitButton()
    {
        if (SaveLoadManager.HasInstance)
        {
            SaveLoadManager.Instance.Save();
        }

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.EndGame();
        }
    }
}
