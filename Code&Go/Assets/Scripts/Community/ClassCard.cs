using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassCard : MonoBehaviour {

    [SerializeField] private Text className;
    [SerializeField] private Text classDescription;
    private ServerClasses.Clase clas;

    public void Configure(ServerClasses.Clase cl) {
        clas = cl;
        className.text = clas.name;
        classDescription.text = clas.description;
    }

    public void PlayClass() {
        CommunityManager.Instance.PlayClass(clas);
    }
}