using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileCard : MonoBehaviour {

    [SerializeField] private Button profileIconButton;
    [SerializeField] private Image profileIconImage;

    [SerializeField] private Text usernameText;
    [SerializeField] private Text teacherRoleText;
    [SerializeField] private Text studentRoleText;

    [SerializeField] private Button changeProfileIconPanelButton;

    [SerializeField] private List<Sprite> profileIconsList;

    public void Start() {
        Configure();
    }

    public void ChangeIcon() {
        changeProfileIconPanelButton.gameObject.SetActive(true);
    }

    public void SelectedIcon(int id) {
        GameManager.Instance.userIconID = id;
        CommunityManager.Instance.ChangeUserProfilePic(id);
        Configure();
    }

    public void Configure() {
        usernameText.text = GameManager.Instance.GetUserName();

        teacherRoleText.gameObject.SetActive(GameManager.Instance.GetIsAdmin());
        studentRoleText.gameObject.SetActive(!GameManager.Instance.GetIsAdmin());

        //roleText.text = GameManager.Instance.GetIsAdmin() ? "Teacher" : "Student";  // TODO localize text
        profileIconImage.sprite = profileIconsList[GameManager.Instance.userIconID];
    }
}
