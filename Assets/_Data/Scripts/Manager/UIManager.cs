using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseManager<UIManager>
{
    [SerializeField] private MenuPanel menuPanel;
    [SerializeField] private SettingPanel settingPanel;
    [SerializeField] private LoadingPanel loadingPanel;
    [SerializeField] private HomeScenePanel homeScenePanel;
    [SerializeField] private TutorialScenePanel tutorialScenePanel;
    [SerializeField] private GamePanel gamePanel;
    [SerializeField] private PausePanel pausePanel;
    [SerializeField] private LosePanel losePanel;
    [SerializeField] private VictoryPanel victoryPanel;
    [SerializeField] private TestPanel testPanel;
    [SerializeField] private GuideCtrl guideCtrl;
    [SerializeField] private Animator animatorTransition;
    [SerializeField] private Canvas canvas;

    public MenuPanel MenuPanel => this.menuPanel;
    public SettingPanel SettingPanel => this.settingPanel;
    public LoadingPanel LoadingPanel => this.loadingPanel;
    public HomeScenePanel HomeScenePanel => this.homeScenePanel;
    public TutorialScenePanel TutorialScenePanel => this.tutorialScenePanel;
    public GamePanel GamePanel => this.gamePanel;
    public PausePanel PausePanel => this.pausePanel;
    public LosePanel LosePanel => this.losePanel;
    public VictoryPanel VictoryPanel => this.victoryPanel;
    public TestPanel TestPanel => this.testPanel;
    public Animator AnimatorTransition => this.animatorTransition;
    public GuideCtrl GuideCtrl => this.guideCtrl;

    //private void Start()
    //{
    //   this.StartMainMenu();
    //}

    private void OnEnable()
    {
        this.StartMainMenu();
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

    public void StartMainMenu()
    {
        this.ActiveMenuPanel(true);
        this.ActiveSettingPanel(false);
        this.ActiveLoadingPanel(false);
        this.ActiveHomeScenePanel(false);
        this.ActiveTutorialScenePanel(false);
        this.ActiveGamePanel(false);
        this.ActivePausePanel(false);
        this.ActiveLosePanel(false);
        this.ActiveVictoryPanel(false);
        this.ActiveTestPanel(false);
        canvas.worldCamera = Camera.main;
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

    public void ActiveHomeScenePanel(bool active)
    {
        this.homeScenePanel.gameObject.SetActive(active);
    }

    public void ActiveTutorialScenePanel(bool active)
    {
        this.tutorialScenePanel.gameObject.SetActive(active);
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

    public void ActiveTestPanel(bool active)
    {
        this.TestPanel.gameObject.SetActive(active);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (this.canvas.worldCamera == null)
        {
            this.canvas.worldCamera = Camera.main;
        }
    }
}
