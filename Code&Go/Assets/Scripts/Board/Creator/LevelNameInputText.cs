using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelNameInputText : MonoBehaviour
{
    private string ip;
    private InputField mainInputField;

    void Start()
    {
        mainInputField = GetComponent<InputField>();
        mainInputField.onEndEdit.AddListener(delegate { EndEditCheck(); });
    }

    void EndEditCheck()
    {
        ip = mainInputField.text;
    }
}
