using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private Animator animatorTransition;

    private void OnEnable()
    {
        if (SaveLoadManager.HasInstance)
        {
            this.resumeButton.SetActive(SaveLoadManager.Instance.saveData.Saved);
        }
    }

    public void OnClickedResumeButton()
    {
        Time.timeScale = 1.0f;

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }

        if (SaveLoadManager.HasInstance)
        {
            if (!SaveLoadManager.Instance.saveData.Saved) return;
        }

        if (SaveLoadManager.HasInstance)
        {
            SaveLoadManager.Instance.Load();
        }
    }

    public void OnClickedNewGameButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }

        if (GameManager.HasInstance)
        {
            StartCoroutine(GameManager.Instance.LoadChangeScene(SceneManager.GetActiveScene().buildIndex + 1));
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
            UIManager.Instance.SettingPanel.SettingMainMenu(true);
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

    public void OnClickedTestButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }

        if (GameManager.HasInstance)
        {
            StartCoroutine(GameManager.Instance.LoadChangeScene(6));
        }
    }
 }
