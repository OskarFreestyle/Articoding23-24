using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameManager : MonoBehaviour
{
    public GameObject blackPanel;
    public GameObject nameMenu;
    public GameObject inputField;
    public GameObject textDisplay;
    private string name;

    // Start is called before the first frame update
    void Start()
    {
        //blackPanel.SetActive(false);
        nameMenu.SetActive(false);
        textDisplay.GetComponent<Text>().text = ProgressManager.Instance.GetName();
    }

    public void SetActiveNamePanel(bool active)
    {
        nameMenu.SetActive(active);
        blackPanel.SetActive(active);
    }

    public void StoreName() {
        name = inputField.GetComponent<Text>().text;
        textDisplay.GetComponent<Text>().text = name;

        ProgressManager.Instance.SetName(name);

        SetActiveNamePanel(false);
    }
}
