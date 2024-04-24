using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FollowingTextUI : MonoBehaviour
{
    [SerializeField] Text objectText;

    public void SetText(string text)
    {
        Debug.Log("Escribiendo " + text);
        objectText.text = text;
    }
}
