using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockToggle : MonoBehaviour
{
    void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(delegate
        {
            restrictions.SetBlockAllow(categoryName, blockName.text);
        });
    }

    public RestrictionsPanel restrictions;
    public Text blockName;
    public string categoryName;
}
