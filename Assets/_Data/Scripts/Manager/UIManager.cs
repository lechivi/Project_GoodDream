using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseManager<UIManager>
{
    [SerializeField] private MenuPanel menuPanel;
    [SerializeField] private SettingPanel settingPanel;
    [SerializeField] private LoadingPanel loadingPanel;
    [SerializeField] private GamePanel gamePanel;
    [SerializeField] private PausePanel pausePanel;
    [SerializeField] private LosePanel losePanel;
    [SerializeField] private VictoryPanel victoryPanel;

    public MenuPanel MenuPanel => this.menuPanel;
    public SettingPanel SettingPanel => this.settingPanel;
    public LoadingPanel LoadingPanel => this.loadingPanel;
    public GamePanel GamePanel => this.gamePanel;
    public PausePanel PausePanel => this.pausePanel;
    public LosePanel LosePanel => this.losePanel;
    public VictoryPanel VictoryPanel => this.victoryPanel;

    private void Start()
    {
        this.ActiveMenuPanel(true);
        this.ActiveSettingPanel(false);
        this.ActiveLoadingPanel(false);
        this.ActiveGamePanel(false);
        this.ActivePausePanel(false);
        this.ActiveLosePanel(false);
        this.ActiveVictoryPanel(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.HasInstance && GameManager.Instance.IsPlaying)
            {
                GameManager.Instance.PauseGame();
                ActivePausePanel(true);
            }
        }
    }

    public void ActiveMenuPanel(bool active)
    {
        this.menuPanel.gameObject.SetActive(active);
    }

    public void ActiveSettingPanel(bool active)
    {
        this.settingPanel.gameObject.SetActive(active);
    }

    public void ActiveLoadingPanel(bool active)
    {
        this.loadingPanel.gameObject.SetActive(active);
    }

    public void ActiveGamePanel(bool active)
    {
        this.gamePanel.gameObject.SetActive(active);
    }

    public void ActivePausePanel(bool active)
    {
        this.pausePanel.gameObject.SetActive(active);
    }

    public void ActiveLosePanel(bool active)
    {
        this.losePanel.gameObject.SetActive(active);
    }

    public void ActiveVictoryPanel(bool active)
    {
        this.victoryPanel.gameObject.SetActive(active);
    }
}
