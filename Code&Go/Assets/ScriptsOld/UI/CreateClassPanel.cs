using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CreateClassPanel : MonoBehaviour
{
    private string newClassName;
    public ActivatedScript activatedScript;

    private void Start()
    {
        GetComponent<InputField>().onEndEdit.AddListener(delegate {
            newClassName = GetComponent<InputField>().text;
        });
    }

    public void TryCreateClass()
    {
        ServerClasses.ClasePost newclass = new ServerClasses.ClasePost();

        newclass.name = newClassName;
        newclass.description = "";
        newclass.studentsId = new List<int>();
        newclass.teachersId = new List<int>();

        activatedScript.Post("classes", JsonUtility.ToJson(newclass), onClassCreateOK, onClassCreateKO);
    }

    private int onClassCreateOK(UnityWebRequest req)
    {
        Debug.Log("Clase creada correctamente.");

        return 0;
    }

    private int onClassCreateKO(UnityWebRequest req)
    {
        Debug.Log("Error: Clase no creada.");

        return 0;
    }
}
