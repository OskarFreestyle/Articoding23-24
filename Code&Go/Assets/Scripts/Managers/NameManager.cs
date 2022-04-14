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
    private string name;

    // Start is called before the first frame update
    void Start()
    {
        //blackPanel.SetActive(false);
        nameMenu.SetActive(false);
        userName.text = ProgressManager.Instance.GetName();
    }

    public void SetActiveNamePanel(bool active)
    {
        nameMenu.SetActive(active);
        blackPanel.SetActive(active);
    }

    public void StoreName() {
        name = inputField.text;
        userName.text = name;

        ProgressManager.Instance.SetName(name);

        SetActiveNamePanel(false);
    }
}
