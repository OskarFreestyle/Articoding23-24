using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedalUIText : MonoBehaviour
{
    [SerializeField] private Transform medalText;
    [SerializeField] private Transform medalTextBG;

    public void activeDescription() {
        medalText.gameObject.SetActive(true);
        medalTextBG.gameObject.SetActive(true);
    }

    public void disableDescription() {
        medalText.gameObject.SetActive(false);
        medalTextBG.gameObject.SetActive(false);
    }
}
