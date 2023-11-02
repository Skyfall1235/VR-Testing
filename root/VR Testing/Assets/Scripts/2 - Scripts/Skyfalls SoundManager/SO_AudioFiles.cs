using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObjects/Game Management/Audio")]
[System.Serializable]
public class SO_AudioFiles : ScriptableObject
{
    [SerializeField] private List<Sounds> soundFiles = new List<Sounds>();

    public Dictionary<string, Sounds> sound = new Dictionary<string, Sounds>();


    //sets up the dictionary
    public void SetUpDictionary()
    {
        for (int i = 0; i < soundFiles.Count; i++)
        {
            sound.Add(soundFiles[i].collectionName, soundFiles[i]);
        }

        //to confirm every soundname has a file

        foreach (var pair in sound)
        {
            Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
        }
    }
}
