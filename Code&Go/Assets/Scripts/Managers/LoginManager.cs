using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour {
    public GameObject waitPanel;
    public GameObject OKPanel;
    public GameObject KOPanel;
    public GameObject loginPanel;
    public GameObject logButton;

    public ActivatedScript activated;

    public ComunidadLayout comunidadLayout;
    public LevelTestManager levelTestManager;

    string userName = "";

    public void TryToLogIn(string user, string pass) {
        if (waitPanel != null) {
            waitPanel.SetActive(true);
        }

        ServerClasses.Login loginJson = new ServerClasses.Login();
        loginJson.username = user;
        loginJson.password = pass;

        userName = user;

        activated.Post("login", JsonUtility.ToJson(loginJson), OnLoginOK, OnLoginKO);
    }

    public void TryToCreateAccount(string user, string pass)
    {
        if (waitPanel != null)
        {
            waitPanel.SetActive(true);
        }

        ServerClasses.CreateAccount createAccountJson = new ServerClasses.CreateAccount();
        createAccountJson.username = user;
        createAccountJson.password = pass;

        userName = user;

        activated.Post("users/students", JsonUtility.ToJson(createAccountJson), OnLoginOK, OnLoginKO);
    }

    public void LogOut() {
        if(comunidadLayout != null)
            comunidadLayout.IsNotLoggedAction();

        Debug.Log("Deslogueado");

        GameManager.Instance.SetToken("");
        GameManager.Instance.SetAdmin(false);
        GameManager.Instance.SetUserName("");
        GameManager.Instance.SetLogged(false);
    }

    int OnLoginOK(UnityWebRequest req) {
        ServerClasses.LoginResponse responseToken = JsonUtility.FromJson<ServerClasses.LoginResponse>(req.downloadHandler.text);
        Debug.Log("Logeado");

        GameManager.Instance.SetToken(responseToken.token);
        GameManager.Instance.SetRole(responseToken.role);
        GameManager.Instance.SetUserName(userName);
        GameManager.Instance.SetLogged(true);

        if (waitPanel != null) {
            waitPanel.SetActive(false);
            OKPanel.SetActive(true);
            KOPanel.SetActive(false);
        }

        if (logButton != null) {
            logButton.SetActive(false);
            levelTestManager.ActivateExportButtons(responseToken.role != "ROLE_USER");
        }

        loginPanel.SetActive(false);

        if (comunidadLayout != null && comunidadLayout.gameObject.active) 
            comunidadLayout.IsLoggedAction();

        return 0;
    }

    int OnLoginKO(UnityWebRequest req)
    {
        Debug.Log("No logeado");
        if (waitPanel != null) {
            waitPanel.SetActive(false);
            KOPanel.SetActive(true);
            OKPanel.SetActive(false);
        }

        userName = "";

        return 0;
    }
}
