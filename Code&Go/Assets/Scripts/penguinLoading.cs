using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class penguinLoading : MonoBehaviour
{
    public Transform initialPoint;
    private float midX = -11.0f;
    private Vector3 temp;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = initialPoint.position;
        temp = new Vector3(-2.0f,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(temp * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Respawn")) {
            transform.position = initialPoint.position;
        }
    }
}