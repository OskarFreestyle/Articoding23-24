using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ComunidadLayout : MonoBehaviour
{
    public GameObject clasesLayout;
    public GameObject notLoggedLayout;

    public Button clasesTab;
    public Button nivelesTab;

    public GameObject clasesList;
    public GameObject nivelesList;

    public ActivatedScript activatedScript;

    ServerClasses.ClaseList clases;

    private void OnEnable()
    {
        if(!GameManager.Instance.GetLogged())
        {
            IsNotLoggedAction();
        }
        else
        {
            IsLoggedAction();
            CreateClasses();
        }
    }

    public void IsLoggedAction()
    {
        clasesLayout.SetActive(true);
        notLoggedLayout.SetActive(false);

        clasesTab.interactable = true;
        nivelesTab.interactable = true;

        CreateClasses();
    }

    public void IsNotLoggedAction()
    {
        clasesLayout.SetActive(false);
        notLoggedLayout.SetActive(true);

        clasesTab.interactable = false;
        nivelesTab.interactable = false;
    }

    void CreateClasses()
    {
        activatedScript.Get("classes", GetClassesOK, GetClassesKO);
    }

    int GetClassesOK(UnityWebRequest req)
    {
        string clasesJson = req.downloadHandler.text;
        //Insertamos este string para poder cogerlo comodamente con jsonutility
        clasesJson = clasesJson.Insert(0, "{ list:");
        clasesJson = clasesJson.Insert(clasesJson.Length, "}");

        try
        {
            clases = JsonUtility.FromJson<ServerClasses.ClaseList>(clasesJson);
        }
        catch
        {
            Debug.Log("Error al leer clases");
        }

        return 0;
    }

    int GetClassesKO(UnityWebRequest req)
    {

        return 0;
    }
}
