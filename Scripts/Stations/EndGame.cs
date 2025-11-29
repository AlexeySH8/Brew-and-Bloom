using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class EndGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _aeonPositionText;
    [SerializeField] private float _typingSpeed;

    private AudioSource _typingSource;
    private MusicManager _musicManager;

    [Inject]
    public void Construct(MusicManager musicManager)
    {
        _musicManager = musicManager;
    }

    private void Awake()
    {
        _typingSource = GetComponent<AudioSource>();
    }

    public void StartEndGame() => StartCoroutine(EndGameRoutine());

    private IEnumerator EndGameRoutine()
    {
        TurnOffAudio();
        yield return PrintAeonPositionText();
        yield return new WaitForSeconds(3);
        Application.Quit();
    }

    private IEnumerator PrintAeonPositionText()
    {
        string text = "A-103";
        int counter = 0;
        foreach (char letters in text)
        {
            _aeonPositionText.text += letters;
            _typingSource.Play();
            counter++;
            yield return new WaitForSeconds(_typingSpeed);
        }
    }

    private void TurnOffAudio()
    {
        float minVolume = 0.01f;
        _musicManager.SetMusicVolume(minVolume);
        SFX.Instance.SetSFXVolume(minVolume);
    }
}
