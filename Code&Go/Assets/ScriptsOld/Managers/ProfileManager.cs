using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public Text starsText;
    public Text levelsText;
    public Text perfectText;
    public Text categoryText;

    void Start() {
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