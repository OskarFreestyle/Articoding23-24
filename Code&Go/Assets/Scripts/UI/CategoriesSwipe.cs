using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CategoriesSwipe : MonoBehaviour {

    [SerializeField] private Color[] colors;
    [SerializeField] private GameObject scrollbar;
    [SerializeField] private GameObject categoriesContent;
    [SerializeField] private GameObject imageContent;
    [SerializeField] private float adjustSpeed;
    [SerializeField] private int initialCategory;

    private float scroll_pos = 0;
    private float[] pos;
    private bool runIt = false;
    private float time;
    private Button takeTheBtn;
    private int btnNumber;

    private void Start() {
        pos = new float[transform.childCount];
        scroll_pos = initialCategory / pos.Length;
        ConfigureCategories(int.MaxValue, true);
    }

    private void Update() {
        ConfigureCategories(adjustSpeed);
    }

    private void ConfigureCategories(float configureSpeed, bool init = false) {
        float distance = 1f / (pos.Length - 1f);

        if (runIt) {
            AdjustList(distance, pos, takeTheBtn, configureSpeed);
            time += Time.deltaTime;

            if (time > configureSpeed) {
                time = 0;
                runIt = false;
            }
        }

        for (int i = 0; i < pos.Length; i++) {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0)) {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        } 
        else if (init) {
            scroll_pos = (float)initialCategory / pos.Length;
        }
        else {
            for (int i = 0; i < pos.Length; i++) {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                    //DOTween.To(() => scrollbar.GetComponent<Scrollbar>().value, x => scrollbar.GetComponent<Scrollbar>().value = x, pos[i], adjustSpeed);
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < pos.Length; i++) {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                // Transform the selected one
                //Debug.LogWarning("Current Category Selected " + i);
                imageContent.transform.GetChild(i).localScale = Vector2.Lerp(categoriesContent.transform.GetChild(i).localScale, new Vector2(1.2f, 1.2f), 0.1f);
                imageContent.transform.GetChild(i).GetComponent<Image>().color = colors[1];
                categoriesContent.transform.GetChild(i).GetComponent<CategoryCard>().Expand(configureSpeed);

                for (int j = 0; j < pos.Length; j++) {
                    // Transform the non selected ones
                    if (j != i) {
                        imageContent.transform.GetChild(j).GetComponent<Image>().color = colors[0];
                        imageContent.transform.GetChild(j).localScale = Vector2.Lerp(imageContent.transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        categoriesContent.transform.GetChild(j).GetComponent<CategoryCard>().Contract(configureSpeed);
                    }
                }
            }
        }
    }

    private void AdjustList(float distance, float[] pos, Button btn, float configureSpeed) {

        for (int i = 0; i < pos.Length; i++) {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                //DOTween.To(() => scrollbar.GetComponent<Scrollbar>().value, x => scrollbar.GetComponent<Scrollbar>().value = x, pos[btnNumber], adjustSpeed);
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[btnNumber], configureSpeed * Time.deltaTime);
            }
        }
    }

    public void WhichShortcutClicked(Button shortcutClicked) {
        string auxName = shortcutClicked.transform.name;
        shortcutClicked.transform.name = "clicked";
        for (int i = 0; i < shortcutClicked.transform.parent.transform.childCount; i++) {
            if (shortcutClicked.transform.parent.transform.GetChild(i).transform.name == "clicked") {
                btnNumber = i;
                takeTheBtn = shortcutClicked;
                time = 0;
                scroll_pos = (pos[btnNumber]);
                runIt = true;
            }
        }
        shortcutClicked.transform.name = auxName;
    }

    public void WhichCategoryClicked(Button categoryClicked) {
        string auxName = categoryClicked.transform.name;
        categoryClicked.transform.name = "clicked";
        for (int i = 0; i < categoryClicked.transform.parent.transform.childCount; i++) {
            if (categoryClicked.transform.parent.transform.GetChild(i).transform.name == "clicked") {
                btnNumber = i;
                takeTheBtn = categoryClicked;
                time = 0;
                scroll_pos = (pos[btnNumber]);
                runIt = true;
            }
        }
        categoryClicked.transform.name = auxName;
    }
}