using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject howToPlayButton;
    [SerializeField] private GameObject howToPlayImage;
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
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }
        if (SaveLoadManager.HasInstance)
        {
            if (!SaveLoadManager.Instance.saveData.Saved) return;
        }
        StartCoroutine(this.ResumeButton());
    }

    public void OnClickedTutorialButton()
    {
        SceneManager.LoadScene(2);
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveMenuPanel(false);
            UIManager.Instance.ActiveGamePanel(true);
        }

    }

    public void OnClickedNewGameButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }
        StartCoroutine(this.PlayButton());
   
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
            GameManager.Instance.EndGame();
        }
    }
    public void OnClickedHowToPlayButton()
    {
        //this.howToPlayImage.SetActive(true);
        //this.howToPlayButton.SetActive(false);
        StartCoroutine(this.PlayButton());
    }

    public void OnClickedOnHowToPlayButtonImage()
    {
        this.howToPlayImage.SetActive(false);
        this.howToPlayButton.SetActive(true);
    }

    private IEnumerator PlayButton()
    {
        this.animatorTransition.Play("Crossfade_Start");

        yield return new WaitForSeconds(1.5f);
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveHomeScenePanel(true);
            //UIManager.Instance.ActiveLoadingPanel(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            UIManager.Instance.ActiveMenuPanel(false);
        }
    }

    private IEnumerator ResumeButton()
    {
        this.animatorTransition.Play("Crossfade_Start");

        yield return new WaitForSeconds(1.5f);
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveMenuPanel(false);
            UIManager.Instance.ActiveGamePanel(true);
        }
        if (GameManager.HasInstance)
        {
            GameManager.Instance.StartGame();
        }
        if (SaveLoadManager.HasInstance)
        {
            SaveLoadManager.Instance.Load();
        }
    }
}
