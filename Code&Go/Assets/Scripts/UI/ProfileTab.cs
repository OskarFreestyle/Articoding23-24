using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileTab : MonoBehaviour {

    [SerializeField] private ProfileManager profileManager;

    private void Start() {
        Debug.Log("Profile tab start");
        profileManager.UpdateUI();
        Debug.Log("Profile tab updated");
    }
}
