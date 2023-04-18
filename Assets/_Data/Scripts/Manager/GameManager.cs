using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;

public class GameManager : BaseManager<GameManager>
{
    private int currentHealth = 0;
    private int maxHealth = 100; 
    private int currentMana = 0;
    private int maxMana = 100;
    private bool isPlaying;

    public int CurrentHealth => this.currentHealth;
    public int MaxHealth => this.maxHealth;   
    public int CurrentMana => this.currentMana;
    public int MaxMana => this.maxMana;
    public bool IsPlaying => this.isPlaying;

    public void UpdateHealth(int value)
    {
        this.currentHealth = value;
    }

    public void UpdateMaxHealth(int value)
    {
        this.maxHealth = value;
    }

    public void UpdateMana(int value)
    {
        this.currentMana = value;
    }

    public void StartGame()
    {
        this.maxHealth = 100;
        this.currentHealth = this.maxHealth;
        this.maxMana = 100; //TODO: Change maxMana for each character type (ScriptableObject)
        this.currentMana = this.maxMana;
        this.isPlaying = true;
        Time.timeScale = 1.0f;

        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.HealthText.SetText(this.currentHealth + "/" + this.maxHealth);
        }
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


    public void RestarGame()
    {
        this.maxHealth = 100;
        this.currentHealth = this.maxHealth;
        this.maxMana = 100; //TODO: Change maxMana for each character type (ScriptableObject)
        this.currentMana = this.maxMana;
        ChangeScene("MenuScene");

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveMenuPanel(true);
            UIManager.Instance.ActiveGamePanel(false);
            UIManager.Instance.ActiveSettingPanel(false);
            UIManager.Instance.ActivePausePanel(false);
            UIManager.Instance.ActiveLosePanel(false);
            UIManager.Instance.ActiveVictoryPanel(false);
            //UIManager.Instance.GamePanel.NumberOfFruits.SetText("0");
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
