using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public class CategoryCard : MonoBehaviour {
    [SerializeField] private Category category;

    [SerializeField] private Text titleText;
    private LocalizeStringEvent titleLocalized;

    [SerializeField] private Text descriptionText;
    private LocalizeStringEvent descriptionLocalized;

    [SerializeField] private Text numStarsText;
    //private LocalizeStringEvent localizedTitle;

    [SerializeField] private Image image;

    /// <summary>
    /// Setup the category using the scriptable object
    /// </summary>
    private void Start() {
        // Set the title
        titleLocalized.StringReference = category.titleLocalized;
        titleLocalized.RefreshString();
        titleText.text = titleLocalized.StringReference.GetLocalizedStringAsync().Result;

        // Set the description
        descriptionLocalized.StringReference = category.descriptionLocalized;
        descriptionLocalized.RefreshString();
        titleText.text = descriptionLocalized.StringReference.GetLocalizedStringAsync().Result;
    }
}
