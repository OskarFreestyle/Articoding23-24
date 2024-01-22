using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComunityLevel : MonoBehaviour
{
    public Text name;
    public Text description;

    public ComunityCurrentCard currentCard;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            currentCard.SetName(name.text);
            currentCard.SetDescription(description.text);
        });
    }
}
