using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockToggle : MonoBehaviour
{
    void Start()
    {
        restrictions.AddBlockToggle(categoryName, this);

        GetComponent<Toggle>().onValueChanged.AddListener(delegate
        {
            restrictions.SetBlockAllow(categoryName, blockName.text);
        });
    }

    //Activa o descactiva el Toggle y devuelve el nombre que esta asignado en el editor
    public string SetActive(bool active)
    {
        GetComponent<Toggle>().isOn = active;
        return blockName.text;
    }

    public RestrictionsPanel restrictions;
    public Text blockName;
    public string categoryName;
}
