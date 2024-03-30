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

    [SerializeField] private Button editButton;

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

    public void EnableEditButton() {
        editButton.gameObject.SetActive(true);
    }

    private void SetText() {
        // Set the title
        try {
            titleLocalized.StringReference = levelData.levelNameLocalized;
            titleLocalized.RefreshString();
            titleText.text = titleLocalized.StringReference.GetLocalizedStringAsync().Result;
        } catch (System.Exception e) {
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
        if(levelData.levelImage) icon.sprite = levelData.levelImage;
    }

    public void PlayLevel() {
        if (levelData) {
            GameManager.Instance.LevelName = levelData.levelName;
            GameManager.Instance.LoadLevel(levelData.categoryData, levelData.index);
        }
        else GameManager.Instance.LoadLevelCreator();
    }

    public void EditLevel() {
        GameManager.Instance.CurrentLevelIndex = levelData.index;
        GameManager.Instance.CurrentCategoryIndex = levelData.categoryData.index;
        EditCreatedLevel();
    }

    public void EditCreatedLevel() {
        LevelDataSO thisleveldata = levelData;

        BoardState thisBoard = JsonUtility.FromJson<BoardState>(thisleveldata.levelBoard.text);
        ActiveBlocks thisActive = JsonUtility.FromJson<ActiveBlocks>(thisleveldata.activeBlocks.text);
        string thisInitial = thisleveldata.customInitialState.text;

        GameManager.Instance.SetCommunityLevelBoard(thisBoard);
        GameManager.Instance.SetCommunityLevelActiveBlocks(thisActive);
        GameManager.Instance.SetCommunityInitialState(thisInitial);
        GameManager.Instance.SetPlayingCommunityLevel(true);

        GameManager.Instance.LoadLevelCreator();
    }
}
