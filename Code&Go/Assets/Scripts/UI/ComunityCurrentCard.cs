using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComunityCurrentCard : MonoBehaviour
{
    public Text nameText;
    public Text description;

    public void SetName(string n)
    {
        nameText.text = n;
    }

    public void SetDescription(string des)
    {
        description.text = des;
    }
}
