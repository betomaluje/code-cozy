using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgSlider;

    private void Start()
    {
        sfxSlider.onValueChanged.AddListener(HandleSFX);
        bgSlider.onValueChanged.AddListener(HandleBG);

        float sfxSavedPrefs = PlayerPrefs.GetFloat(SoundManager.PREFS_SFX, SoundManager.instance.GetVolumeForType(SoundType.SFX));
        float songSavedPrefs = PlayerPrefs.GetFloat(SoundManager.PREFS_SONG, SoundManager.instance.GetVolumeForType(SoundType.SONG));

        sfxSlider.value = sfxSavedPrefs;
        bgSlider.value = songSavedPrefs;
    }

    private void HandleSFX(float volume)
    {
        SoundManager.instance.SetVolumeForType(SoundType.SFX, volume);

        PlayerPrefs.SetFloat(SoundManager.PREFS_SFX, volume);
    }

    private void HandleBG(float volume)
    {
        SoundManager.instance.SetVolumeForType(SoundType.SONG, volume);
        PlayerPrefs.SetFloat(SoundManager.PREFS_SONG, volume);
    }
}
