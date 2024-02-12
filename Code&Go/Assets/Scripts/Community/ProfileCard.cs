using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileCard : MonoBehaviour {

    [SerializeField] private Button profileIconButton;


    [SerializeField] private Button changeProfileIconPanelButton;

    public void ChangeIcon() {
        changeProfileIconPanelButton.gameObject.SetActive(true);
    }
}
