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

    private void OnEnable()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Scene_Home"); //TODO: Change it when done design game
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            this.loadingSlider.value = asyncOperation.progress;
            this.loadingPercentText.SetText($"LOADING SCENES: {asyncOperation.progress * 100}%");

            if (asyncOperation.progress >= 0.9f) //0 - 0.9: load scene. 0.9 - 1: chuyen scene
            {
                this.loadingSlider.value = 1f;
                this.loadingPercentText.SetText("Press any button to continue");

                if (Input.anyKeyDown)
                {
                    if (SceneManager.GetActiveScene().buildIndex == 0)
                    {
                        StartCoroutine(GameObject.Find("LevelLoader").GetComponent<LevelLoader>().SetTriggerFadeOut());
                    }

                    asyncOperation.allowSceneActivation = true;
                    if (UIManager.HasInstance)
                    {
                        UIManager.Instance.ActiveGamePanel(true);
                        UIManager.Instance.ActiveLoadingPanel(false);
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

}
