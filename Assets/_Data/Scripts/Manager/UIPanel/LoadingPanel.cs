using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingPercentText;
    [SerializeField] private Slider loadingSlider;

    public IEnumerator LoadScene(int sceneIndex)
    {
        this.loadingSlider.value = 0;
        this.loadingPercentText.SetText("Loading");
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            this.loadingSlider.value = asyncOperation.progress;
            this.loadingPercentText.SetText($"Loading: {(int)asyncOperation.progress * 100}%");

            if (asyncOperation.progress >= 0.9f) //0 - 0.9: load scene. 0.9 - 1: chuyen scene
            {
                this.loadingSlider.value = 1f;
                this.loadingPercentText.SetText("Press any button to continue");

                if (Input.anyKeyDown)
                {
                    asyncOperation.allowSceneActivation = true;
                    if (UIManager.HasInstance)
                    {
                        UIManager.Instance.ActiveLoadingPanel(false);
                        UIManager.Instance.AnimatorTransition.Rebind();

                        if (sceneIndex == 0) //MainMenu
                        {
                            UIManager.Instance.StartMainMenu();
                        }
                        else if (sceneIndex == 1) //HomeScene
                        {
                            UIManager.Instance.ActiveHomeScenePanel(true);
                            UIManager.Instance.ActiveMenuPanel(false);
                        }
                        else if (sceneIndex == 2) //TutorialScene
                        {
                            UIManager.Instance.ActiveHomeScenePanel(false);
                            UIManager.Instance.ActiveGamePanel(true);
                        }
                        else if (sceneIndex == 6) //TestScene
                        {
                            UIManager.Instance.ActiveTestPanel(true);
                            UIManager.Instance.ActiveGamePanel(true);
                            UIManager.Instance.ActiveMenuPanel(false);
                        }
                        else //PlayScene
                        {
                            UIManager.Instance.ActiveMenuPanel(false);
                            UIManager.Instance.ActiveGamePanel(true);
                        }
                    }

                    if (GameManager.HasInstance)
                    {
                        GameManager.Instance.StartGame();
                    }
                }
            }
            yield return null;

        }
    }

    //public IEnumerator LoadScene(string sceneName)
    //{
    //    this.loadingSlider.value = 0;
    //    this.loadingPercentText.SetText("Loading");
    //    yield return null;

    //    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
    //    asyncOperation.allowSceneActivation = false;
    //    while (!asyncOperation.isDone)
    //    {
    //        this.loadingSlider.value = asyncOperation.progress;
    //        this.loadingPercentText.SetText($"Loading: {asyncOperation.progress * 100}%");

    //        if (asyncOperation.progress >= 0.9f) //0 - 0.9: load scene. 0.9 - 1: chuyen scene
    //        {
    //            this.loadingSlider.value = 1f;
    //            this.loadingPercentText.SetText("Press any button to continue");

    //            if (Input.anyKeyDown)
    //            {
    //                asyncOperation.allowSceneActivation = true;
    //                if (UIManager.HasInstance)
    //                {
    //                    UIManager.Instance.ActiveLoadingPanel(false);
    //                    UIManager.Instance.AnimatorTransition.Rebind();

    //                    //if (sceneIndex == 1)
    //                    //{
    //                    //    UIManager.Instance.ActiveHomeScenePanel(true);
    //                    //    UIManager.Instance.ActiveMenuPanel(false);
    //                    //}
    //                    //else if (sceneIndex == 2)
    //                    //{
    //                    //    UIManager.Instance.ActiveHomeScenePanel(false);
    //                    //    UIManager.Instance.ActiveGamePanel(true);
    //                    //}
    //                }

    //                if (GameManager.HasInstance)
    //                {
    //                    GameManager.Instance.StartGame();
    //                }
    //            }
    //        }
    //        yield return null;

    //    }
    //}

}
