using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public class LevelCard : MonoBehaviour {

    [SerializeField] private Image cardBGImage;

    [SerializeField] private Text titleText;
    [SerializeField] private LocalizeStringEvent titleLocalized;
    [SerializeField] private Image titleBGImage;

    [SerializeField] private Image icon;

    [SerializeField] private Image starIcon0;
    [SerializeField] private Image starIcon1;
    [SerializeField] private Image starIcon2;

    [SerializeField] private Image buttonBGImage;

    private LevelDataSO levelData;

    public void SetLevelData(LevelDataSO levelData) {
        Debug.Log("Setted: " + levelData.name);
        this.levelData = levelData;
        SetText();
        SetColors();
        SetIcon();

        // TODO
        SetStars();
    }
    private void SetStars() {

    }

    private void SetText() {
        // Set the title
        titleLocalized.StringReference = levelData.levelNameLocalized;
        titleLocalized.RefreshString();
        titleText.text = titleLocalized.StringReference.GetLocalizedStringAsync().Result;
    }

    private void SetColors() {
        // Set the primary color
        cardBGImage.color = levelData.categoryData.primaryColor;

        // Set the secondary color
        titleBGImage.color = levelData.categoryData.secondaryColor;
    }

    private void SetIcon() {
        // Set the icon
        icon.sprite = levelData.levelPreview;
    }
}
