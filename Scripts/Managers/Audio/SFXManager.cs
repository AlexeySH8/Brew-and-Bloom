using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private AudioSource _sfxSource;

    private void Awake()
    {
        _sfxSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(AudioClip audioClip)
    {
        if (audioClip != null)
            _sfxSource.PlayOneShot(audioClip);
    }
}
