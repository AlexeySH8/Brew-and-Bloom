using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class SFX : MonoBehaviour
{
    public static SFX Instance { get; private set; }
    [SerializeField] private AudioMixer _mixer;

    [Header("General")]
    [SerializeField] private AudioClip _nightSound;
    [SerializeField] private AudioClip _clickButtonDefault;
    [SerializeField] private AudioClip _clickButtonBuy;
    [SerializeField] private AudioClip _clickButtonCancel;
    [SerializeField] private AudioClip _clickButtonNewDay;
    [SerializeField] private AudioClip _clickButtonNewGame;
    [Header("Player")]
    [SerializeField] private AudioClip _pickupItem;
    [SerializeField] private AudioClip _dropItem;
    [Header("GrowingPlant")]
    [SerializeField] private AudioClip _plantSeed;
    [SerializeField] private AudioClip _plantGetHarvest;
    [SerializeField] private AudioClip _plantNeedWater;
    [SerializeField] private AudioClip _plantDriedOut;
    [Header("Tools")]
    [SerializeField] private AudioClip _waterMagic;
    [SerializeField] private AudioClip _fireMagic;
    [SerializeField] private AudioClip[] _chops;
    [SerializeField] private AudioClip[] _mines;

    private const string SFX_VOLUME = "sfxVolume";
    private const string SFX_MIXER = "SFXVolume";
    private AudioSource _sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate ItemPool detected, destroying new one.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _sfxSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        LoadVolume();
    }

    public void SetSFXVolume(float volume)
    {
        _mixer.SetFloat(SFX_MIXER, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(SFX_VOLUME, volume);
        PlayerPrefs.Save();
    }

    #region General
    public void PlayClickButtonDefault() => _sfxSource.PlayOneShot(_clickButtonDefault);
    public void PlayClickButtonBuy() => _sfxSource.PlayOneShot(_clickButtonBuy);
    public void PlayClickButtonClose() => _sfxSource.PlayOneShot(_clickButtonCancel);
    public void PlayClickButtonNewDay() => _sfxSource.PlayOneShot(_clickButtonNewDay);
    public void PlayClickButtonNewGame() => _sfxSource.PlayOneShot(_clickButtonNewGame);
    #endregion

    #region Player
    public void PlayPickUpItem() => _sfxSource.PlayOneShot(_pickupItem);
    public void PlayDropItem() => _sfxSource.PlayOneShot(_dropItem);
    #endregion

    #region GrowingPlant
    public void PlayPlantSeed() => _sfxSource.PlayOneShot(_plantSeed);
    public void PlayPlantGetHarvest() => _sfxSource.PlayOneShot(_plantGetHarvest);
    public void PlayPlantNeedWater() => _sfxSource.PlayOneShot(_plantNeedWater);
    public void PlayPlantDriedOut() => _sfxSource.PlayOneShot(_plantDriedOut);
    #endregion

    #region Tools
    public void PlayChop() => _sfxSource.PlayOneShot(_chops[Random.Range(0, _chops.Length)]);
    public void PlayMine() => _sfxSource.PlayOneShot(_mines[Random.Range(0, _mines.Length)]);
    public void PlayWaterMagic() => _sfxSource.PlayOneShot(_waterMagic);
    public void PlayFireMagic() => _sfxSource.PlayOneShot(_fireMagic);
    #endregion

    public void PlayAudioClip(AudioClip audioClip)
    {
        if (audioClip != null)
            _sfxSource.PlayOneShot(audioClip);
    }

    private void LoadVolume()
    {
        var volume = PlayerPrefs.GetFloat(SFX_VOLUME, 0.5f);
        SetSFXVolume(volume);
    }
}
