using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClasesManager : MonoBehaviour
{
    public List<GameObject> clases;
    public GameObject[] lists;

    public int tabIndex;

    private void SelectCallback(int index)
    {
        if (index >= clases.Count || index < 0) return;

        // Desactivamos el tab actual
        if (tabIndex != index) lists[tabIndex].SetActive(false);

        // Activamos el nuevo tab seleccionado
        tabIndex = index;
        lists[tabIndex].SetActive(true);
    }

    public void ConfigureTabs()
    {
        for (int i = 0; i < clases.Count; i++)
        {
            int index = i;

            clases[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                SelectCallback(index);
            });
        }
    }
}
