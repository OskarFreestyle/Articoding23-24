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
    //private LocalizeStringEvent localizedTitle;
    [SerializeField] private Image numStarsBGImage;

    [SerializeField] private Image icon;
    [SerializeField] private Image iconBGImage;

    [SerializeField] private Image buttonBGImage;

    /// <summary>
    /// Setup the category using the scriptable object
    /// </summary>
    private void Start() {
        Debug.Log("STARTED");
        SetColors();
        SetTexts();
        SetIcon();
        Debug.Log("FINISHED");
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
        numStarsText.text = "0/" + category.GetTotalStars();
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
}
