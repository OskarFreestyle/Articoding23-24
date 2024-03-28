using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassesDisplay : MonoBehaviour {

    [SerializeField] private ClassCard classCardTemplate;

    public void InstanciateClassCards() {
        ClearDisplay();

        foreach(ServerClasses.Clase clase in CommunityManager.Instance.Classes.content) {

            ClassCard classCard = Instantiate(classCardTemplate, transform);
            classCard.Configure(clase);

        }
    }

    private void ClearDisplay() {
        // Clear all the levelCards
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }
}