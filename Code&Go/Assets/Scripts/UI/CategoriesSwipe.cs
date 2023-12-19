using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoriesSwipe : MonoBehaviour {

    [SerializeField] private Color[] colors;
    [SerializeField] private GameObject scrollbar;
    [SerializeField] private GameObject imageContent;

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

            if (time > 4f) {
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
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }


        for (int i = 0; i < pos.Length; i++) {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                Debug.LogWarning("Current Selected Level" + i);
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                imageContent.transform.GetChild(i).localScale = Vector2.Lerp(imageContent.transform.GetChild(i).localScale, new Vector2(1.2f, 1.2f), 0.1f);
                imageContent.transform.GetChild(i).GetComponent<Image>().color = colors[1];
                for (int j = 0; j < pos.Length; j++) {
                    if (j != i) {
                        imageContent.transform.GetChild(j).GetComponent<Image>().color = colors[0];
                        imageContent.transform.GetChild(j).localScale = Vector2.Lerp(imageContent.transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
    }

    private void AdjustList(float distance, float[] pos, Button btn) {

        for (int i = 0; i < pos.Length; i++) {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[btnNumber], 1f * Time.deltaTime);
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