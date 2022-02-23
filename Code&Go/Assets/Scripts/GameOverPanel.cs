using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Localization;
using UnityEngine.Localization.Settings;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private Text _errorText;
    [SerializeField] private StringTableCollection _stringTableCollection;

    private int _languageCode;
    private void Awake()
    {
        if (LocalizationSettings.SelectedLocale.Identifier.Code == "en") _languageCode = 0;
        else _languageCode = 1;
    }

    public void SetLocalizedText(string entry)
    {
        try
        {
            _errorText.text = _stringTableCollection.StringTables[_languageCode][entry].LocalizedValue;
        }
        catch (Exception e)
        {
            _errorText.gameObject.SetActive(false);
        }

    }
}
