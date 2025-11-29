using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour
{
    private const string LOCALE_KEY = "localeKey";
    private Coroutine _settingLocale;

    private void Awake()
    {
        string code = PlayerPrefs.GetString(LOCALE_KEY, "en");
        ChangeLocale(code);
    }

    public void ChangeLocale(string code)
    {
        if (_settingLocale != null) return;

        _settingLocale = StartCoroutine(SetLocale(code));
    }

    private IEnumerator SetLocale(string code)
    {
        yield return LocalizationSettings.InitializationOperation;

        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale.Identifier.Code == code)
            {
                LocalizationSettings.SelectedLocale = locale;
                PlayerPrefs.SetString(LOCALE_KEY, code);
                PlayerPrefs.Save();
                break;
            }
        }
        _settingLocale = null;
    }
}
