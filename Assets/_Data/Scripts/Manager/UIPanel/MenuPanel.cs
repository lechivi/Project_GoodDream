using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private GameObject howToPlayButton;
    [SerializeField] private GameObject howToPlayImage;
    [SerializeField] private Animator animatorTransition;
    public void OnStartButtonClick()
    {
        StartCoroutine(this.PlayButton());
   
    }

    public void OnSettingButtonClick()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveSettingPanel(true);
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

        if (AudioManager.HasInstance)
        {
            //AudioManager.Instance.PlayBGM(AUDIO.BGM_BGM_02, 0.5f);
        }
    }
}
