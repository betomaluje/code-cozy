using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;

    public static SoundManager instance;

    [HideInInspector] public const string PREFS_SFX = "sound_sfx";
    [HideInInspector] public const string PREFS_SONG = "sound_song";

    private Sound lastThemeSong;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.alternatives.Add(s.clip);

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.spatialBlend = s.spatialBlend;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }

        RestoreVolumes();

        PlayOnAwake();
    }

    public Sound GetSound(string name)
    {
        return Array.Find(sounds, sound => sound.name == name);
    }

    private void RestoreVolumes()
    {
        float sfxSavedPrefs = PlayerPrefs.GetFloat(PREFS_SFX, instance.GetVolumeForType(SoundType.SFX));
        float songSavedPrefs = PlayerPrefs.GetFloat(PREFS_SONG, instance.GetVolumeForType(SoundType.SONG));

        SetVolumeForType(SoundType.SFX, sfxSavedPrefs);
        SetVolumeForType(SoundType.SONG, songSavedPrefs);
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.source == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }

        if (s.alternatives != null && s.alternatives.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, s.alternatives.Count);
            Sound copy = s;
            copy.source.clip = s.alternatives[randomIndex];
            copy.source.Play();
            copy = null;
        }
        else
        {
            s.source.Play();
        }
    }

    private void PlayOnAwake()
    {
        Sound[] s = Array.FindAll(sounds, sound => sound.playOnAwake);

        foreach (var sound in s)
        {
            Play(sound.name);
        }
    }

    public void PlayReversed(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.source == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }

        s.source.timeSamples = s.source.clip.samples - 1;
        s.source.pitch = -1;

        s.source.Play();
    }

    public void PlayWithPitch(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }

        s.source.pitch = pitch;
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }

        s.source.Stop();
    }

    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    public void StopAllBackground()
    {
        Sound[] soundsOfType =
            Array.FindAll(sounds, sound => sound.soundType == SoundType.SONG && sound.source != null);
        foreach (Sound s in soundsOfType)
        {
            s.source.Stop();
        }
    }

    public void PauseCurrentThemeSong()
    {
        if (lastThemeSong != null)
        {
            Debug.Log("Pausing song " + lastThemeSong.name);
            lastThemeSong.source.Pause();
        }
    }

    public void ResumeCurrentThemeSong()
    {
        if (lastThemeSong != null)
        {
            Debug.Log("Resuming song " + lastThemeSong.name);
            lastThemeSong.source.Play();
        }
    }

    public void SetVolumeForType(SoundType soundType, float volume)
    {
        Sound[] soundsOfType = Array.FindAll(sounds, sound => sound.soundType == soundType && sound.source != null);
        if (soundsOfType == null || soundsOfType.Length <= 0)
        {
            Debug.LogWarning("Sounds of type " + soundType.ToString() + " not found");
            return;
        }

        foreach (Sound s in soundsOfType)
        {
            s.source.volume = volume;
        }
    }

    public float GetVolumeForType(SoundType soundType)
    {
        Sound[] soundsOfType = Array.FindAll(sounds, sound => sound.soundType == soundType && sound.source != null);
        if (soundsOfType == null || soundsOfType.Length <= 0)
        {
            Debug.LogWarning("Sounds of type " + soundType.ToString() + " not found");
            return 0.4f;
        }

        soundsOfType = soundsOfType.OrderBy(x => x.volume).ToArray();

        return soundsOfType[0].volume;
    }

    public void PitchEverything(float time, float pitch = .5f)
    {
        StartCoroutine(Pitch(time, pitch));
    }

    private IEnumerator Pitch(float time, float pitch = .5f)
    {
        Hashtable savedPitches = new Hashtable();
        foreach (Sound s in sounds)
        {
            if (s.source.isPlaying)
            {
                savedPitches.Add(s.name, s.source.pitch);
                s.source.pitch = pitch;
            }
        }

        yield return new WaitForSeconds(time);

        foreach (Sound s in sounds)
        {
            if (savedPitches.ContainsKey(s.name))
            {
                float previousPitch = (float) savedPitches[s.name];
                s.source.pitch = previousPitch;
            }
        }
    }
}