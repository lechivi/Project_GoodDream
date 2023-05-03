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
        Debug.Log("Pause");
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


    public void RestarGame()
    {
        ChangeScene("MainMenu");

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveMenuPanel(true);
            UIManager.Instance.ActiveGamePanel(false);
            UIManager.Instance.ActiveSettingPanel(false);
            UIManager.Instance.ActivePausePanel(false);
            UIManager.Instance.ActiveLosePanel(false);
            UIManager.Instance.ActiveVictoryPanel(false);
        }
    }
    public void EndGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
