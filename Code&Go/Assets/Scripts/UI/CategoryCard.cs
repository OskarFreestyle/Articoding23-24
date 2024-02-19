using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using DG.Tweening;

public class CategoryCard : MonoBehaviour {

    [SerializeField] private CategoryDataSO category;

    public CategoryDataSO Category {
        get { return category; }
        private set { category = value; }
    }

    [SerializeField] private Image cardBGImage;

    [SerializeField] private Text titleText;
    [SerializeField] private LocalizeStringEvent titleLocalized;
    [SerializeField] private Image titleBGImage;

    [SerializeField] private Text descriptionText;
    [SerializeField] private LocalizeStringEvent descriptionLocalized;
    [SerializeField] private Image descriptionBGImage;

    [SerializeField] private Text numStarsText;
    [SerializeField] private Image numStarsBGImage;

    [SerializeField] private Image icon;
    [SerializeField] private Image lockedIcon;
    [SerializeField] private Image iconBGImage;

    [SerializeField] private Image buttonBGImage;

    [SerializeField] private LevelsDisplay levelDisplay;
    [SerializeField] private TabGroup tabGroup;
    [SerializeField] private TabButton sandboxTabButton;

    private const int EXPAND_WIDTH = 640;
    private const int EXPAND_HEIGHT = 700;
    private const float EXPAND_TEXTS_SCALE = 1.0f;
    private const float EXPAND_ICON_SCALE = 1.0f;    
    private const float EXPAND_DESCRIPTION_SCALE = 1.0f;
    private const int EXPAND_NUM_STARS_POS_X = 10;
    private const int EXPAND_NUM_STARS_POS_Y = -330;
    private const int EXPAND_BUTTON_POS_X = -300;
    private const int EXPAND_CREATED_LEVEL_BUTTON_POS_X = -145;
    private const int EXPAND_BUTTON_POS_Y = -330;    
    private const int EXPAND_DESCRIPTION_POS_X = -300;
    private const int EXPAND_DESCRIPTION_POS_Y = -240;
    private const int EXPAND_ICON_POS_X = 0;
    private const int EXPAND_ICON_POS_Y = 70;    
    
    private const int CONTRACT_WIDTH = 400;
    private const int CONTRACT_HEIGHT = 438;
    private const float CONTRACT_TEXTS_SCALE = 0.6f;
    private const float CONTRACT_ICON_SCALE = 0.8f;
    private const float CONTRACT_DESCRIPTION_SCALE = 0.25f;
    private const float CONTRACT_CREATED_LEVELS_DESCRIPTION_SCALE = 0f;
    private const int CONTRACT_NUM_STARS_POS_X = -78;
    private const int CONTRACT_NUM_STARS_POS_Y = -199;    
    private const int CONTRACT_BUTTON_POS_X = -78;
    private const int CONTRACT_BUTTON_POS_Y = -199;    
    private const int CONTRACT_DESCRIPTION_POS_X = -68;
    private const int CONTRACT_DESCRIPTION_POS_Y = -195;
    private const int CONTRACT_ICON_POS_X = 0;
    private const int CONTRACT_ICON_POS_Y = 0;

    /// <summary>
    /// Setup the category using the scriptable object
    /// </summary>
    private void Start() {
        SetColors();
        SetTexts();
        SetIcon();
        SetButton();
        Configure();
    }

    private void SetButton() {
        // Set the button function
        buttonBGImage.GetComponent<Button>().onClick.AddListener(OnSelectedButtonClicked);
    }

    private void OnSelectedButtonClicked() {
        if (category.index != 0) {
            levelDisplay.InstanciateLevelsFromCategory(category);
        } else {
            tabGroup.OnTabSelected(sandboxTabButton);
        }
    }

    private void SetIcon() {
        // Set the current icon
        icon.sprite = category.icon;
    }

    private void SetTexts() {
        // Set the title
        titleLocalized.StringReference = category.titleLocalized;
        titleLocalized.RefreshString();
        titleText.text = titleLocalized.StringReference.GetLocalizedStringAsync().Result;

        // Set the description
        descriptionLocalized.StringReference = category.descriptionLocalized;
        descriptionLocalized.RefreshString();
        descriptionText.text = descriptionLocalized.StringReference.GetLocalizedStringAsync().Result;

        // The created levels category doesn't have stars
        if (category.index == 0) numStarsBGImage.gameObject.SetActive(false);

        // Set the stars number by default
        //numStarsText.text = "0/" + category.GetTotalStars();
    }

    private void SetColors() {
        // Set the primary color
        cardBGImage.color = category.primaryColor;

        // Set the secondary color
        titleBGImage.color = category.secondaryColor;
        descriptionBGImage.color = category.secondaryColor;
        numStarsBGImage.color = category.secondaryColor;
        buttonBGImage.color = category.secondaryColor;
        iconBGImage.color = category.secondaryColor;
    }

    public void Expand(float adjustSpeed) {
        // Scale the category card
        DOTween.To(() => GetComponent<RectTransform>().sizeDelta, x => GetComponent<RectTransform>().sizeDelta = x, new Vector2(EXPAND_WIDTH, EXPAND_HEIGHT), adjustSpeed);

        // Scale the title text
        DOTween.To(() => titleBGImage.GetComponent<RectTransform>().localScale, x => titleBGImage.GetComponent<RectTransform>().localScale = x, new Vector3(EXPAND_TEXTS_SCALE, EXPAND_TEXTS_SCALE), adjustSpeed);
        
        // Scale and move the icon
        DOTween.To(() => iconBGImage.GetComponent<RectTransform>().localScale, x => iconBGImage.GetComponent<RectTransform>().localScale = x, new Vector3(EXPAND_ICON_SCALE, EXPAND_ICON_SCALE), adjustSpeed);
        DOTween.To(() => iconBGImage.GetComponent<RectTransform>().localPosition, x => iconBGImage.GetComponent<RectTransform>().localPosition = x, new Vector3(EXPAND_ICON_POS_X, EXPAND_ICON_POS_Y), adjustSpeed);

        // Scale and move the description
        DOTween.To(() => descriptionBGImage.GetComponent<RectTransform>().localScale, x => descriptionBGImage.GetComponent<RectTransform>().localScale = x, new Vector3(EXPAND_DESCRIPTION_SCALE, EXPAND_DESCRIPTION_SCALE), adjustSpeed);
        DOTween.To(() => descriptionBGImage.GetComponent<RectTransform>().localPosition, x => descriptionBGImage.GetComponent<RectTransform>().localPosition = x, new Vector3(EXPAND_DESCRIPTION_POS_X, EXPAND_DESCRIPTION_POS_Y), adjustSpeed);

        // Scale, move and enable the button
        buttonBGImage.GetComponent<Button>().enabled = true;
        DOTween.To(() => buttonBGImage.GetComponent<RectTransform>().localScale, x => buttonBGImage.GetComponent<RectTransform>().localScale = x, new Vector3(EXPAND_TEXTS_SCALE, EXPAND_TEXTS_SCALE), adjustSpeed);
        if(category.index != 0) {
            DOTween.To(() => buttonBGImage.GetComponent<RectTransform>().localPosition, x => buttonBGImage.GetComponent<RectTransform>().localPosition = x, new Vector3(EXPAND_BUTTON_POS_X, EXPAND_BUTTON_POS_Y), adjustSpeed);
        }
        else {
            DOTween.To(() => buttonBGImage.GetComponent<RectTransform>().localPosition, x => buttonBGImage.GetComponent<RectTransform>().localPosition = x, new Vector3(EXPAND_CREATED_LEVEL_BUTTON_POS_X, EXPAND_BUTTON_POS_Y), adjustSpeed);
        }

        // Scale and move the num stars
        DOTween.To(() => numStarsBGImage.GetComponent<RectTransform>().localScale, x => numStarsBGImage.GetComponent<RectTransform>().localScale = x, new Vector3(EXPAND_TEXTS_SCALE, EXPAND_TEXTS_SCALE), adjustSpeed);
        DOTween.To(() => numStarsBGImage.GetComponent<RectTransform>().localPosition, x => numStarsBGImage.GetComponent<RectTransform>().localPosition = x, new Vector3(EXPAND_NUM_STARS_POS_X, EXPAND_NUM_STARS_POS_Y), adjustSpeed);
    }

    public void Contract(float adjustSpeed) {
        // Scale the category card
        DOTween.To(() => GetComponent<RectTransform>().sizeDelta, x => GetComponent<RectTransform>().sizeDelta = x, new Vector2(CONTRACT_WIDTH, CONTRACT_HEIGHT), adjustSpeed);

        // Scale the title text
        DOTween.To(() => titleBGImage.GetComponent<RectTransform>().localScale, x => titleBGImage.GetComponent<RectTransform>().localScale = x, new Vector3(CONTRACT_TEXTS_SCALE, CONTRACT_TEXTS_SCALE), adjustSpeed);

        // Scale and move the icon
        DOTween.To(() => iconBGImage.GetComponent<RectTransform>().localScale, x => iconBGImage.GetComponent<RectTransform>().localScale = x, new Vector3(CONTRACT_ICON_SCALE, CONTRACT_ICON_SCALE), adjustSpeed);
        DOTween.To(() => iconBGImage.GetComponent<RectTransform>().localPosition, x => iconBGImage.GetComponent<RectTransform>().localPosition = x, new Vector3(CONTRACT_ICON_POS_X, CONTRACT_ICON_POS_Y), adjustSpeed);

        // Scale and move the description
        if (category.index != 0) {
            DOTween.To(() => descriptionBGImage.GetComponent<RectTransform>().localScale, x => descriptionBGImage.GetComponent<RectTransform>().localScale = x, new Vector3(CONTRACT_DESCRIPTION_SCALE, CONTRACT_DESCRIPTION_SCALE), adjustSpeed / 2);
            DOTween.To(() => descriptionBGImage.GetComponent<RectTransform>().localPosition, x => descriptionBGImage.GetComponent<RectTransform>().localPosition = x, new Vector3(CONTRACT_DESCRIPTION_POS_X, CONTRACT_DESCRIPTION_POS_Y), adjustSpeed / 2);
        }
        else {
            DOTween.To(() => descriptionBGImage.GetComponent<RectTransform>().localScale, x => descriptionBGImage.GetComponent<RectTransform>().localScale = x, new Vector3(CONTRACT_CREATED_LEVELS_DESCRIPTION_SCALE, CONTRACT_CREATED_LEVELS_DESCRIPTION_SCALE), adjustSpeed / 2);
            DOTween.To(() => descriptionBGImage.GetComponent<RectTransform>().localPosition, x => descriptionBGImage.GetComponent<RectTransform>().localPosition = x, new Vector3(CONTRACT_CREATED_LEVELS_DESCRIPTION_SCALE, CONTRACT_DESCRIPTION_POS_Y), adjustSpeed / 2);
        }

        // Scale, move and enable the button
        buttonBGImage.GetComponent<Button>().enabled = false;
        if (category.index != 0) {
            DOTween.To(() => buttonBGImage.GetComponent<RectTransform>().localPosition, x => buttonBGImage.GetComponent<RectTransform>().localPosition = x, new Vector3(CONTRACT_BUTTON_POS_X, CONTRACT_BUTTON_POS_Y), adjustSpeed / 2);
            DOTween.To(() => buttonBGImage.GetComponent<RectTransform>().localScale, x => buttonBGImage.GetComponent<RectTransform>().localScale = x, new Vector3(CONTRACT_TEXTS_SCALE, CONTRACT_TEXTS_SCALE), adjustSpeed / 2);
        }
        else {
            DOTween.To(() => buttonBGImage.GetComponent<RectTransform>().localPosition, x => buttonBGImage.GetComponent<RectTransform>().localPosition = x, new Vector3(CONTRACT_CREATED_LEVELS_DESCRIPTION_SCALE, CONTRACT_BUTTON_POS_Y), adjustSpeed / 2);
            DOTween.To(() => buttonBGImage.GetComponent<RectTransform>().localScale, x => buttonBGImage.GetComponent<RectTransform>().localScale = x, new Vector3(CONTRACT_CREATED_LEVELS_DESCRIPTION_SCALE, CONTRACT_CREATED_LEVELS_DESCRIPTION_SCALE), adjustSpeed / 2);
        }
        // Scale and move the num stars
        DOTween.To(() => numStarsBGImage.GetComponent<RectTransform>().localScale, x => numStarsBGImage.GetComponent<RectTransform>().localScale = x, new Vector3(CONTRACT_TEXTS_SCALE, CONTRACT_TEXTS_SCALE), adjustSpeed);
        DOTween.To(() => numStarsBGImage.GetComponent<RectTransform>().localPosition, x => numStarsBGImage.GetComponent<RectTransform>().localPosition = x, new Vector3(CONTRACT_NUM_STARS_POS_X, CONTRACT_NUM_STARS_POS_Y), adjustSpeed);
    }

    public void Configure() {
        // The created levels category doesn't need to be configured
        if (category.index == 0) return;

        // Get the current data
        CategorySaveData categoryData = ProgressManager.Instance.GetCategoryData(category.index);

        // Update the number of stars
        numStarsText.text = categoryData.GetCurrentNumStars().ToString() + "/" + category.GetTotalStars().ToString();

        // Update the icon
        lockedIcon.gameObject.SetActive(categoryData.levelsData[0] == -1);
        icon.gameObject.SetActive(categoryData.levelsData[0] != -1);
        buttonBGImage.GetComponent<Button>().interactable = categoryData.levelsData[0] != -1;
        //Debug.Log(category.name + " button state " + (categoryData.levelsData[0] != -1));
    }
}
