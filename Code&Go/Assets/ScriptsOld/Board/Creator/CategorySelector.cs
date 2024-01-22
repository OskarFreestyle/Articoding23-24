using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategorySelector : MonoBehaviour
{
    private Dropdown dropDown;
    public LevelTestManager levelTestManager;

    private void Start()
    {
        dropDown = GetComponent<Dropdown>();

        dropDown.onValueChanged.AddListener(delegate { ChangeCategory(dropDown.value); });
    }

    private void ChangeCategory(int value)
    {
        levelTestManager.ChangeLevelCategory(value);
    }
}
