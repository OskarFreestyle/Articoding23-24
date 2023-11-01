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

    private void Start() {
        // Start positioning the penguin on the initial position
        transform.position = initialPoint.position;
    }

    private void Update() {
        // Move the penguin
        transform.position += Vector3.right * speed * Time.deltaTime;

        // Reposition the penguin when it passes the final position
        if(transform.position.x >= finalPoint.position.x) {
            transform.position = initialPoint.position;
        }
    }
}