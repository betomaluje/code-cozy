using UnityEngine;

public class SceneThemeSound : MonoBehaviour
{
    [SerializeField] private string ThemeSound;

    private void Start()
    {
        SoundManager.instance.StopAllBackground();
        SoundManager.instance.Play(ThemeSound);
        SoundManager.instance.Play("Ambience_Forest");
    }
}