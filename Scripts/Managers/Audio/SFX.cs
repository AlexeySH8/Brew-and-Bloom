using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public static SFX Instance { get; private set; }

    private AudioSource _sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _sfxSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(AudioClip audioClip)
    {
        if (audioClip != null)
            _sfxSource.PlayOneShot(audioClip);
    }
}
