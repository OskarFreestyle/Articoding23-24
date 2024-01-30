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
    [SerializeField] private Image lockedIcon;

    [SerializeField] private Image starIcon0;
    [SerializeField] private Image starIcon1;
    [SerializeField] private Image starIcon2;

    [SerializeField] private Image buttonBGImage;

    private LevelDataSO levelData;

    public void SetLevelData(LevelDataSO levelData) {
        this.levelData = levelData;
        SetText();
        SetColors();
        SetIcon();
    }

    public void SetLevelStars(int levelStars) {
        if(levelStars < 3) starIcon2.color = new Color (0,0,0,0.5f);
        if(levelStars < 2) starIcon1.color = new Color (0,0,0,0.5f);
        if(levelStars < 1) starIcon0.color = new Color (0,0,0,0.5f);

        // Level locked
        if (levelStars < 0) {
            starIcon0.color = new Color(0, 0, 0, 0.5f);
            lockedIcon.gameObject.SetActive(true);
            buttonBGImage.GetComponent<Button>().enabled = false;
        }
    }

    public void DisableStars() {
        starIcon0.gameObject.SetActive(false);
        starIcon1.gameObject.SetActive(false);
        starIcon2.gameObject.SetActive(false);
    }

    private void SetText() {
        // Set the title
        try {
            titleLocalized.StringReference = levelData.levelNameLocalized;
            titleLocalized.RefreshString();
            titleText.text = titleLocalized.StringReference.GetLocalizedStringAsync().Result;
        } catch (System.Exception e) {
            Debug.Log("Created level so text is ");
            titleText.text = levelData.levelName;
        }
    }

    private void SetColors() {
        // Set the primary color
        cardBGImage.color = levelData.categoryData.primaryColor;

        // Set the secondary color
        titleBGImage.color = levelData.categoryData.secondaryColor;
        buttonBGImage.color = levelData.categoryData.secondaryColor;
    }

    private void SetIcon() {
        // Set the icon
        if(levelData.levelPreview) icon.sprite = levelData.levelPreview;
    }

    public void PlayLevel() {
        GameManager.Instance.LoadLevel(levelData.categoryData, levelData.index);
    }
}
