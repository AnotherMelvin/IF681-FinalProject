using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayAudio(int index)
    {
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    public void PlayLocalAudio(int index, AudioSource audioSource)
    {
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }
}
