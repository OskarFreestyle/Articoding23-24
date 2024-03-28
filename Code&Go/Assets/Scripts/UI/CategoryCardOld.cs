using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CategoryCardOld : MonoBehaviour {
    [SerializeField] private CategoryDataSO category;

    [SerializeField] private LocalizeStringEvent localizeString;
    [SerializeField] private Text title;
    [SerializeField] private Text stars;

    [SerializeField] private ProgressBar progressBar;

    [HideInInspector] public Button button;
    public Image image;

    public void ConfigureCategory(CategoryDataSO category) {
        this.category = category;
    }
}
