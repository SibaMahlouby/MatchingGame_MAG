//To play the music and sound effects identified in SOundID

using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Serializable]
    private class SoundIDClipPair
    {
        public SoundID SoundID;
        public AudioClip AudioClip;
    }

    [SerializeField]
    private SoundIDClipPair[] soundIDClipPairs;

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource effectSource;

    private readonly Dictionary<SoundID, AudioClip> soundIDToClipMap = new Dictionary<SoundID, AudioClip>();

    private void Start()
    {
        InitializeSoundIDToClipMap();
    }

    private void InitializeSoundIDToClipMap()
    {
        foreach (var soundIDClipPair in soundIDClipPairs)
        {
            soundIDToClipMap.Add(soundIDClipPair.SoundID, soundIDClipPair.AudioClip);
        }
    }

    private void PlayMusic(AudioClip audioClip, bool looping = true)
    {
        if (musicSource.isPlaying) return;

        musicSource.clip = audioClip;
        musicSource.loop = looping;
        musicSource.Play();
    }

    private void PlayEffect(AudioClip audioClip)
    {
        effectSource.PlayOneShot(audioClip);
    }

    public void PlayEffect(SoundID soundID)
    {
        if (soundID == SoundID.None) return;

        var audioClip = soundIDToClipMap[soundID];
        PlayEffect(audioClip);
    }
}
