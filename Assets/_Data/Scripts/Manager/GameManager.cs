using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;

public class GameManager : BaseManager<GameManager>
{
    private bool isPlaying;

    public bool IsPlaying => this.isPlaying;

    public void StartGame()
    {
        this.isPlaying = true;
        Time.timeScale = 1.0f;
    }

    public void PauseGame()
    {
        if (this.isPlaying)
        {
            this.isPlaying = false;
            Time.timeScale = 0.0f;
        }
    }

    public void ResumeGame()
    {
        this.isPlaying = true;
        Time.timeScale = 1.0f;
    }

    public void BackToMainMenu()
    {
        this.ResumeGame();
        if (SaveLoadManager.HasInstance)
        {
            SaveLoadManager.Instance.Save();
        }

        StartCoroutine(this.LoadChangeScene(0));
    }

    public void QuitGame()
    {
        if (SaveLoadManager.HasInstance)
        {
            SaveLoadManager.Instance.Save();
        }

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public IEnumerator ChangeScene(string sceneName)
    {
        UIManager.Instance.AnimatorTransition.SetTrigger("End");

        yield return new WaitForSeconds(1.25f);
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator ChangeScene(int sceneIndex)
    {
        UIManager.Instance.AnimatorTransition.SetTrigger("End");

        yield return new WaitForSeconds(1.25f);
        SceneManager.LoadScene(sceneIndex);
    }

    //public IEnumerator LoadChangeScene(string sceneName)
    //{
    //    UIManager.Instance.AnimatorTransition.SetTrigger("End");

    //    yield return new WaitForSeconds(1.25f);
    //    UIManager.Instance.ActiveLoadingPanel(true);
    //    StartCoroutine(UIManager.Instance.LoadingPanel.LoadScene(sceneName));
    //}

    public IEnumerator LoadChangeScene(int sceneIndex)
    {
        UIManager.Instance.AnimatorTransition.SetTrigger("End");

        yield return new WaitForSeconds(1.25f);
        UIManager.Instance.ActiveLoadingPanel(true);
        StartCoroutine(UIManager.Instance.LoadingPanel.LoadScene(sceneIndex));
        yield return null;
    }

}
