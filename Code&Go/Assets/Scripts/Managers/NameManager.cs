using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameManager : MonoBehaviour
{
    public GameObject blackPanel;
    public GameObject nameMenu;

    // Start is called before the first frame update
    void Start()
    {
        //blackPanel.SetActive(false);
        nameMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveNamePanel(bool active)
    {
        nameMenu.SetActive(active);
        blackPanel.SetActive(active);

/*        if (active)
            TrackerAsset.Instance.Accessible.Accessed("name_panel", AccessibleTracker.Accessible.Screen);
        else
            TrackerAsset.Instance.GameObject.Interacted("name_panel_close_button");*/
    }
}
