using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour {
    public Text username;
    public InputField password;

    public LoginManager loginManager;

    private void Start() {
        GetComponent<Button>().onClick.AddListener(delegate { TryToLogin(username.text, password.text); });
    }

    private void TryToLogin(string user, string pass)
    {
        loginManager.TryToLogIn(user, pass);
        password.text = "";
    }
}
