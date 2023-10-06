using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinLoadingBehaviour : MonoBehaviour
{
    [SerializeField] private Transform initialPoint;
    [SerializeField] private Transform finalPoint;
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = initialPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Respawn")) {
            transform.position = initialPoint.position;
        }
    }
}