using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CategoriesSwipe : MonoBehaviour {

    [SerializeField] private Color[] colors;
    [SerializeField] private GameObject scrollbar;
    [SerializeField] private GameObject imageContent;
    [SerializeField] private float adjustSpeed;

    private float scroll_pos = 0;
    private float[] pos;
    private bool runIt = false;
    private float time;
    private Button takeTheBtn;
    int btnNumber;

    private void Update() {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);

        if (runIt) {
            AdjustList(distance, pos, takeTheBtn);
            time += Time.deltaTime;

            if (time > adjustSpeed) {
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
        else {
            for (int i = 0; i < pos.Length; i++) {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], adjustSpeed);
                }
            }
        }

        for (int i = 0; i < pos.Length; i++) {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                // Transform the selected one
                Debug.LogWarning("Current Category Selected " + i);
                imageContent.transform.GetChild(i).GetComponent<CategoryCard>().Expand(adjustSpeed);

                for (int j = 0; j < pos.Length; j++) {
                    // Transform the non selected ones
                    if (j != i) {
                        imageContent.transform.GetChild(j).GetComponent<CategoryCard>().Contract(adjustSpeed);
                    }
                }
            }
        }
    }

    private void AdjustList(float distance, float[] pos, Button btn) {

        for (int i = 0; i < pos.Length; i++) {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[btnNumber], adjustSpeed * Time.deltaTime);
            }
        }

        for (int i = 0; i < btn.transform.parent.transform.childCount; i++) {
            btn.transform.name = ".";
        }

    }


    public void WhichShortcutClicked(Button shortcutClicked) {
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
    }

    public void WhichCategoryClicked(Button categoryClicked) {
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
    }
}