using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The penguin behaviour during the Loading Scene
/// </summary>
public class PenguinLoadingBehaviour : MonoBehaviour
{
    [SerializeField] private Transform initialPoint;
    [SerializeField] private Transform finalPoint;
    [SerializeField] private float speed;

    private void Start()
    {
        transform.position = initialPoint.position;
    }

    private void Update()
    {
        Debug.Log("PenguinLoadingBehaviour Update");
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Respawn")) {
            Debug.Log("Resposition");
            transform.position = initialPoint.position;
        }
    }
}