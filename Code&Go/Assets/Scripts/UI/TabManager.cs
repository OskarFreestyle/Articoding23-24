using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public Tab[] tabs;
    public GameObject[] panels;

    public int tabIndex;

    void Start()
    {
        ConfigureTabs();
        tabs[tabIndex].Select();
    }

    private void SelectCallback(int index)
    {
        if (index >= tabs.Length || index < 0) return;

        // Desactivamos el tab actual
        if(tabIndex != index) tabs[tabIndex].Deselect();

        // Activamos el nuevo tab seleccionado
        tabIndex = index;
        panels[tabIndex].SetActive(true);
    }
    private void DeselectCallback(int index)
    {
        if (index >= tabs.Length || index < 0) return;

        panels[index].SetActive(false);
    }

    private void ConfigureTabs()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            int index = i;

            tabs[i].callbacks.OnSelected.AddListener(() =>
            {
                SelectCallback(index);
            });

            tabs[i].callbacks.OnDeselected.AddListener(() =>
            {
                DeselectCallback(index);
            });

            tabs[i].Deselect();
        }
    }
}
