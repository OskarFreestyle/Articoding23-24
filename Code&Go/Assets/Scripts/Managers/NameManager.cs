using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameManager : MonoBehaviour
{
    public GameObject blackPanel;
    public GameObject nameMenu;
    public Text inputField;
    public Text userName;

    void Start() {
        nameMenu.SetActive(false);
        userName.text = ProgressManager.Instance.Name;
    }

    public void SetActiveNamePanel(bool active) {
        nameMenu.SetActive(active);
        blackPanel.SetActive(active);
    }

    public void StoreName() {
        name = inputField.text;
        userName.text = name;

        ProgressManager.Instance.Name = name;
        SaveManager.Instance.Save();

        SetActiveNamePanel(false);

    }
}
