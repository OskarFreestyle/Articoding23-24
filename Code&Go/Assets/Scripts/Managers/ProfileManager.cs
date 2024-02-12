using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour {

    // Medals
    [SerializeField] private List<Image> medals;
    [SerializeField] private Color activeMedalColor;
    [SerializeField] private Color disactiveMedalColor;

    // Panels texts
    [SerializeField] private Text starsText;
    [SerializeField] private Text levelsText;
    [SerializeField] private Text perfectText;
    [SerializeField] private Text categoryText;

    void Start() {
        Debug.Log("Profile Manager Start");
        SetPanelsTexts();
        SetMedals();
    }

    private void SetMedals() {
        int total = ProgressManager.Instance.GetTotalFinishedCategories() - 1;  // -1 because of the created category
        Debug.Log("Total categories finished: " + total);

        int i = 0;
        foreach(Image medal in medals) {
            medal.color = (i < total) ? activeMedalColor : disactiveMedalColor;
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