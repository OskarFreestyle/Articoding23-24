using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Tab Button that changes its color and text
/// </summary>
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private TabGroup tabGroup;

    [SerializeField] private Image background;
    public Image Background {
        get { return background; }
    }

    [SerializeField] private Text tabText;
    public Text TabText {
        get { return tabText; }
    }

    public void OnPointerClick(PointerEventData eventData) {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData) {
        tabGroup.OnTabExit(this);
    }
}
