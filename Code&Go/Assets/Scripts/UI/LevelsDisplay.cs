using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public class LevelsDisplay : MonoBehaviour {

    [SerializeField] private Text titleText;
    [SerializeField] private LocalizeStringEvent titleLocalized;
    [SerializeField] private Image titleBGImage;

    [SerializeField] private LevelCard levelCardTemplate;
    [SerializeField] private List<Vector3> levelsLocalPositions;

    public void InstanciateLevelsFromCategory(CategoryDataSO category) {
        // Active the levels page
        transform.parent.gameObject.SetActive(true);

        // Set the title
        titleLocalized.StringReference = category.titleLocalized;
        titleLocalized.RefreshString();
        titleText.text = titleLocalized.StringReference.GetLocalizedStringAsync().Result;
        titleBGImage.color = category.secondaryColor;

        int i = 0;
        foreach(LevelDataSO levelData in category.levels) {
            LevelCard currentLevelCard = Instantiate(levelCardTemplate, transform);
            currentLevelCard.SetLevelData(levelData);
            currentLevelCard.transform.localPosition = levelsLocalPositions[i];
            i++;
        }
    }

    public void ClearDisplay() {
        // Clear all the levelCards
        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }

        // Then disable the gameObject
        transform.parent.gameObject.SetActive(false);
    }

}
