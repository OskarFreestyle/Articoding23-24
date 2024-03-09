using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour {

    private static LoginManager instance;
    static public LoginManager Instance {
        get { return instance; }
    }

    [SerializeField] private RectTransform loginPage;
    [SerializeField] private RectTransform mainPage;
    [SerializeField] private ProfileCard profileCard;

    public GameObject waitPanel;
    public GameObject OKPanel;
    public GameObject KOPanel;
    public GameObject loginPanel;
    public GameObject logButton;

    public ActivatedScript activated;

    string userName = "";

    private void Awake() {
        Debug.Log("Login Manager Awake");

        if (!instance) {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            Debug.LogWarning("More than 1 Login Manager created");
            DestroyImmediate(gameObject);
        }

        Debug.Log("Login Manager Awake Finished");
    }

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
        Debug.Log("Deslogueado");

        // Conexion variables
        GameManager.Instance.SetToken("");
        GameManager.Instance.SetAdmin(false);
        GameManager.Instance.SetUserName("");
        GameManager.Instance.SetLogged(false);

        loginPage.gameObject.SetActive(true);
        mainPage.gameObject.SetActive(false);
    }

    int OnLoginOK(UnityWebRequest req) {
        Debug.Log("Logeado");

        ServerClasses.LoginResponse responseToken = JsonUtility.FromJson<ServerClasses.LoginResponse>(req.downloadHandler.text);

        // Conexion variables
        GameManager.Instance.SetToken(responseToken.token);
        GameManager.Instance.SetRole(responseToken.role);
        GameManager.Instance.SetUserName(userName);
        GameManager.Instance.SetLogged(true);

        if (waitPanel != null) {
            waitPanel.SetActive(false);
            OKPanel.SetActive(true);
            KOPanel.SetActive(false);
        }

        loginPanel.SetActive(false);

        loginPage.gameObject.SetActive(false);
        mainPage.gameObject.SetActive(true);

        profileCard.Configure();

        CommunityManager.Instance.GetUserLikedLevels();

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
