using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClasesManager : MonoBehaviour
{
    public GameObject[] clases;
    public GameObject[] lists;

    public int tabIndex;

    void Start()
    {
        ConfigureTabs();
    }

    private void SelectCallback(int index)
    {
        if (index >= clases.Length || index < 0) return;

        // Desactivamos el tab actual
        if (tabIndex != index) lists[tabIndex].SetActive(false);

        // Activamos el nuevo tab seleccionado
        tabIndex = index;
        lists[tabIndex].SetActive(true);
    }

    private void ConfigureTabs()
    {
        for (int i = 0; i < clases.Length; i++)
        {
            int index = i;

            clases[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                SelectCallback(index);
            });
        }
    }
}
