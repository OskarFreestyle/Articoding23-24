using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IPInputText : MonoBehaviour
{
    private string text;
    private InputField mainInputField;

    void Start()
    {
        mainInputField = GetComponent<InputField>();
        mainInputField.onEndEdit.AddListener(delegate { EndEditCheck(); });
    }

    void EndEditCheck()
    {
        text = mainInputField.text;
    }

    public string GetString()
    {
        return text;
    }
}
