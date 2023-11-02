using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement; // Temp Functionality

public class SoundManager : MonoBehaviour
{
    [SerializeField] SO_AudioFiles audioFiles;
    [SerializeField] SO_SettingData settingsData;
    //use a dictionary and combine the 2 during load
    private void Awake()
    {
        audioFiles.SetUpDictionary();
    }
    //play sound, at sound location
    public void PlaySoundAtLocation(SoundType type, string soundName, int indexLocation, AudioSource source, float rawVolume)
    {
        float volume = TrueVolume(rawVolume, type);
        AudioClip chosenSound;
        if (audioFiles.sound.TryGetValue(soundName, out Sounds chosenStruct))
        {
            chosenSound = chosenStruct.soundFile[indexLocation];
            source.PlayOneShot(chosenSound, volume);
            Debug.Log($"Sound {soundName} played successfully");
        }
        else
        {
            Debug.LogWarning("Sound " + soundName + "was not found in dictionary 'sounds'");
        }
    }

    private float TrueVolume(float raw, SoundType type)
    {
        float volumeValue = 0;
        switch (type)
        {
            case SoundType.Music:
                Console.WriteLine("Playing music");
                volumeValue  = raw * settingsData.musicVolume;
                break;
            case SoundType.SoundEffect:
                Console.WriteLine("Playing sound effect");
                volumeValue = raw * settingsData.soundEffectVolume;
                break;
        }
        return volumeValue;
    }
}