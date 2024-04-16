using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the tab buttons
/// </summary>
public class TabGroup : MonoBehaviour {

    [SerializeField] private List<TabButton> tabButtons;
    private TabButton selectedTabButton;

    [SerializeField] private List<GameObject> objectsToSwap;

    [SerializeField] private Color tabActive;
    [SerializeField] private Color tabIdle;
    [SerializeField] private Color tabHover;

    [SerializeField] private Color tabActiveText;
    [SerializeField] private Color tabIdleText;

    private void Start() {
        if(GameManager.Instance.IsPlayingCommunityLevel() && tabButtons.Count >=5) OnTabSelected(tabButtons[4]);
        else OnTabSelected(tabButtons[0]);
    }

    public void OnTabEnter(TabButton tabButton) {
        ResetTabs();
        if (selectedTabButton == null || selectedTabButton != tabButton) {
            tabButton.Background.color = tabHover;
        }
    }

    public void OnTabExit(TabButton tabButton) {
        ResetTabs();
    }

    public void OnTabSelected(TabButton tabButton) {
        selectedTabButton = tabButton;
        ResetTabs();
        tabButton.Background.color = tabActive;
        tabButton.TabText.color = tabActiveText;
        int index = tabButton.transform.GetSiblingIndex();
        for(int i = 0; i < objectsToSwap.Count; i++) {
            if(i == index) {
                objectsToSwap[i].SetActive(true);
            }
            else {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs() {
        foreach (TabButton tabButton in tabButtons) {
            if (selectedTabButton != null && selectedTabButton == tabButton) continue;
            tabButton.Background.color = tabIdle;
            tabButton.TabText.color = tabIdleText;
        }
    }
}
