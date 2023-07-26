using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public SoundType soundType;

    public AudioClip clip;

    public List<AudioClip> alternatives;

    [Range(0f, 1f)] public float volume = .5f;

    [Range(.1f, 3f)] public float pitch = 1;

    [Range(0f, 1f)] public float spatialBlend;

    public bool loop;
    public bool playOnAwake;

    [HideInInspector] public AudioSource source;
}