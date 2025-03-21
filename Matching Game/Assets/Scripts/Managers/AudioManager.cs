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

    // Initializes the audio dictionary on startup.
    private void Start()
    {
        InitializeSoundIDToClipMap();
    }

    // Populates the dictionary for quick sound lookups.
    private void InitializeSoundIDToClipMap()
    {
        foreach (var soundIDClipPair in soundIDClipPairs)
        {
            soundIDToClipMap.Add(soundIDClipPair.SoundID, soundIDClipPair.AudioClip);
        }
    }

    // Plays background music.
    private void PlayMusic(AudioClip audioClip, bool looping = true)
    {
        if (musicSource.isPlaying) return;

        musicSource.clip = audioClip;
        musicSource.loop = looping;
        musicSource.Play();
    }

    //Plays a sound effect
    private void PlayEffect(AudioClip audioClip)
    {
        effectSource.PlayOneShot(audioClip);
    }


   // Plays a sound effect based on SoundID.
    public void PlayEffect(SoundID soundID)
    {
        if (soundID == SoundID.None) return;

        var audioClip = soundIDToClipMap[soundID];
        PlayEffect(audioClip);
    }
}
