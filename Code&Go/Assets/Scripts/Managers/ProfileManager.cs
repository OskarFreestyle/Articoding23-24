using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour {

    // Name
    [SerializeField] private GameObject blackPanel;
    [SerializeField] private GameObject nameMenu;
    [SerializeField] private Text inputField;
    [SerializeField] private Text userName;

    // Medals
    [SerializeField] private List<Image> medals;
    [SerializeField] private Color activeMedalColor;
    [SerializeField] private Color disactiveMedalColor;

    // Panels texts
    [SerializeField] private Text starsText;
    [SerializeField] private Text levelsText;
    [SerializeField] private Text perfectText;
    [SerializeField] private Text categoryText;

    public void UpdateUI() {
        SetPanelsTexts();
        SetMedals();
        SetName();
    }

    private void SetName() {
        Debug.Log("Name segun progress manager: " + ProgressManager.Instance.Name);
        nameMenu.SetActive(false);
        if (ProgressManager.Instance.Name != "") userName.text = ProgressManager.Instance.Name;
        else userName.text = "Pengu";
        //userName.text = ProgressManager.Instance.Name != "" ? "Pengu" : ProgressManager.Instance.Name;
    }

    public void SetActiveNamePanel(bool active) {
        nameMenu.SetActive(active);
        blackPanel.SetActive(active);
    }
    public void StoreName() {
        // Non empty names
        if (inputField.text == "") return;

        name = inputField.text;
        userName.text = name;

        ProgressManager.Instance.Name = name;
        SaveManager.Instance.Save();

        SetActiveNamePanel(false);
    }

    private void SetMedals() {
        // Enable or disable the medals checking the progress manager
        bool[] perfectCategories = ProgressManager.Instance.GetPerfectFinishedCategories();  // -1 because of the created category
        int i = 1;
        foreach(Image medal in medals) {
            medal.color = perfectCategories[i] ? activeMedalColor : disactiveMedalColor;
            i++;
        }
    }

    private void SetPanelsTexts() {
        // Set the stars number
        starsText.text = ProgressManager.Instance.GetTotalStars().ToString();

        // Set the total complete levels number
        levelsText.text = ProgressManager.Instance.GetTotalLevelsComplete().ToString();

        // Set the percentage of perfect levels completed
        int totalLevels = ProgressManager.Instance.GetTotalLevels();
        if (totalLevels > 0) {
            perfectText.text = ((ProgressManager.Instance.GetTotalPerfectLevelsComplete() * 100) / totalLevels).ToString() + "%";
        }
        else {
            perfectText.text = "0%";
        }

        // Set the last category text -1 because of the created levels category
        categoryText.text = (ProgressManager.Instance.GetTotalFinishedCategories() - 1).ToString();
    }
}