public class InGameMenu : BaseMenu
{
    private ConditionsManager _conditionsManager;

    private void Awake()
    {
        _conditionsManager = FindObjectOfType<ConditionsManager>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _conditionsManager.OnGameOver += HandleGameOver;

        _playerInput.OpenMenuEvent += HandleOpenMenu;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _conditionsManager.OnGameOver -= HandleGameOver;

        _playerInput.OpenMenuEvent -= HandleOpenMenu;
    }

    private void HandleOpenMenu()
    {
        ShowPage(typeof(OptionsPage));
    }

    private void HandleGameOver()
    {
        ShowPage(typeof(GameOverPage));
    }

    public void Click_Restart()
    {
        sceneLoader.RestartCurrentScene();
    }

    public void Click_ExitOptions()
    {
        HidePage(typeof(OptionsPage));
    }

    public void Click_Quit()
    {
        sceneLoader.ChangeScene("MainMenu");
    }
}