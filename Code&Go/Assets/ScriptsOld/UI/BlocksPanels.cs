using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksPanels : MonoBehaviour
{
    public GameObject[] blockPanels;

    public void ActivatePanel(int category)
    {
        for(int i = 0; i < blockPanels.Length; i++)
        {
            blockPanels[i].SetActive(i==category);
        }
    }
}
