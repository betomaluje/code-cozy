using UnityEditor;
using UnityEngine;

public class MainMenu : BaseMenu
{
    private void Awake()
    {
        ShowPage(typeof(MainPage));
    }
    
    public void Click_Start() { sceneLoader.ChangeScene("SampleScene"); }

    public void Click_Options() { ShowPage(typeof(OptionsPage)); }

    public void Click_Credits() { }

    public void Click_Back() { ShowPage(typeof(MainPage)); }

    public void Click_Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
