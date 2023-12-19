using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryToggleOld : MonoBehaviour
{
    void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(delegate
        {
            restrictions.SetCategoryAllow(categoryName.text);
        });
    }

    public RestrictionsPanel restrictions;
    public Text categoryName; 
}
