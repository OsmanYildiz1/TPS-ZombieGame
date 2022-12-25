using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Footstep Sources")]
    public AudioClip[] footStepSounds;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private AudioClip GetRandomFootStep()
    {
        return footStepSounds[UnityEngine.Random.Range(0, footStepSounds.Length)]; 
    }
    private void Step()
    {
        AudioClip clip = GetRandomFootStep();
        audioSource.PlayOneShot(clip);
    }
}
