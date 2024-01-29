using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Gauntlet : MonoBehaviour
{
    [SerializeField] private SelectedStoneType type;
    private bool gauntIsActive = false;
    [SerializeField] private Vector3[] stonePositions;
    [SerializeField] private ParticleSystem PTCLsystem;
    [SerializeField] private AudioClip[] audioFX;
    [SerializeField] private AudioSource audioSource;

    public void ActivateStone(SelectedStoneType type)
    {

    }

    public void InitGauntlet()
    {
        //play magic FX
        PlayFX();
        gauntIsActive = true;
    }
    private void PlayFX()
    {
        PTCLsystem.Play();
        //ring
        audioSource.PlayOneShot(audioFX[0], 0.6f);
        //whoosh
        audioSource.PlayOneShot(audioFX[1], 1f);
    }

    
    private void ResetStoneSingle(int stoneID)
    {

    }
    private void ResetStoneMultiple()
    {

    }

    public enum SelectedStoneType
    {
        None,
        fire,
        water,
        earth
    }
}
