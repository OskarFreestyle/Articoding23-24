using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FollowingTextUI : MonoBehaviour {

    [SerializeField] private Text objectText;

    public void SetText(string text) {
        objectText.text = text;
    }

}
