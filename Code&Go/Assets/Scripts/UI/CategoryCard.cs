using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public class CategoryCard : MonoBehaviour {

    [SerializeField] private Category category;

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
    [SerializeField] private Image iconBGImage;

    [SerializeField] private Image buttonBGImage;

    private const int EXPAND_WIDTH = 640;
    private const int EXPAND_HEIGHT = 700;
    private const float EXPAND_TEXTS_SCALE = 1.0f;
    private const float EXPAND_ICON_SCALE = 1.0f;
    private const int EXPAND_NUM_STARS_POS_X = 10;
    private const int EXPAND_NUM_STARS_POS_Y = -330;
    private const int EXPAND_ICON_POS_X = 0;
    private const int EXPAND_ICON_POS_Y = 70;    
    
    private const int CONTRACT_WIDTH = 400;
    private const int CONTRACT_HEIGHT = 438;
    private const float CONTRACT_TEXTS_SCALE = 0.6f;
    private const float CONTRACT_ICON_SCALE = 0.8f;
    private const int CONTRACT_NUM_STARS_POS_X = -78;
    private const int CONTRACT_NUM_STARS_POS_Y = -199;
    private const int CONTRACT_ICON_POS_X = 0;
    private const int CONTRACT_ICON_POS_Y = 0;

    /// <summary>
    /// Setup the category using the scriptable object
    /// </summary>
    private void Start() {
        SetColors();
        SetTexts();
        SetIcon();
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

        // Set the stars number
        numStarsText.text = ProgressManager.Instance.GetCategoryTotalStars(category).ToString() + "/" + category.GetTotalStars();
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
        Debug.Log(transform.name + " expanded");

        // Disable the description and the button
        descriptionBGImage.gameObject.SetActive(true);
        buttonBGImage.gameObject.SetActive(true);

        // Scale the category card
        GetComponent<RectTransform>().sizeDelta = new Vector2(EXPAND_WIDTH, EXPAND_HEIGHT);

        // Scale the title text
        titleBGImage.GetComponent<RectTransform>().localScale = new Vector2(EXPAND_TEXTS_SCALE, EXPAND_TEXTS_SCALE);
        
        // Scale and move the icon
        iconBGImage.GetComponent<RectTransform>().localScale = new Vector2(EXPAND_ICON_SCALE, EXPAND_ICON_SCALE);
        iconBGImage.GetComponent<RectTransform>().localPosition = new Vector2(EXPAND_ICON_POS_X, EXPAND_ICON_POS_Y);

        // Scale and move the num stars
        numStarsBGImage.GetComponent<RectTransform>().localScale = new Vector2(EXPAND_TEXTS_SCALE, EXPAND_TEXTS_SCALE);
        numStarsBGImage.GetComponent<RectTransform>().localPosition = new Vector2(EXPAND_NUM_STARS_POS_X, EXPAND_NUM_STARS_POS_Y);
    }

    public void Contract(float adjustSpeed) {
        Debug.Log(transform.name + " contracted");

        // Disable the description and the button
        descriptionBGImage.gameObject.SetActive(false);
        buttonBGImage.gameObject.SetActive(false);

        // Scale the category card
        GetComponent<RectTransform>().sizeDelta = new Vector2(CONTRACT_WIDTH, CONTRACT_HEIGHT);

        // Scale the title text
        titleBGImage.GetComponent<RectTransform>().localScale = new Vector2(CONTRACT_TEXTS_SCALE, CONTRACT_TEXTS_SCALE);

        // Scale and move the icon
        iconBGImage.GetComponent<RectTransform>().localScale = new Vector2(CONTRACT_ICON_SCALE, CONTRACT_ICON_SCALE);
        iconBGImage.GetComponent<RectTransform>().localPosition = new Vector2(CONTRACT_ICON_POS_X, CONTRACT_ICON_POS_Y);

        // Scale and move the num stars
        numStarsBGImage.GetComponent<RectTransform>().localScale = new Vector2(CONTRACT_TEXTS_SCALE, CONTRACT_TEXTS_SCALE);
        numStarsBGImage.GetComponent<RectTransform>().localPosition = new Vector2(CONTRACT_NUM_STARS_POS_X, CONTRACT_NUM_STARS_POS_Y);
    }
}
