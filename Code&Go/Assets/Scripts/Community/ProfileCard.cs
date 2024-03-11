using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileCard : MonoBehaviour {

    [SerializeField] private Button profileIconButton;

    [SerializeField] private Text usernameText;
    [SerializeField] private Text roleText;

    [SerializeField] private Button changeProfileIconPanelButton;

    public void Start() {
        Configure();
    }

    public void ChangeIcon() {
        changeProfileIconPanelButton.gameObject.SetActive(true);
    }

    public void Configure() {
        usernameText.text = GameManager.Instance.GetUserName();
        roleText.text = GameManager.Instance.GetIsAdmin() ? "Teacher" : "Student";  // TODO localize text
    }
}
