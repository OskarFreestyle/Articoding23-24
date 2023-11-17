using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCategory : MonoBehaviour
{
    private InputField inputField;
    public Text levelNameText;
    public LevelTestManager levelTestManager;

    private void Start()
    {
        inputField = GetComponent<InputField>();

        inputField.onEndEdit.AddListener(delegate { ChangeLevelName(); });
    }

    private void ChangeLevelName()
    {
        levelTestManager.ChangeLevelName(levelNameText.text);
    }
}