using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCircle : MonoBehaviour {
    [SerializeField] private Image image;

    [SerializeField] private Color levelsColor;
    [SerializeField] private Color playlistColor;

    [SerializeField] private float timeToRotate;
    private float timeToRotateAux;
    private bool move = false;

    //private void Update() {
    //    timeToRotateAux += Time.deltaTime;

    //    if(timeToRotateAux >= timeToRotate) {
    //        timeToRotateAux -= timeToRotate;
    //        transform.Rotate(new Vector3(0, 0,-360 / 12));
    //    }
    //}

    public void Show(bool isLevelsColor) {
        if (isLevelsColor) image.color = levelsColor;
        else image.color = playlistColor;
        gameObject.SetActive(true);
        move = true;
        StartCoroutine(Move());
    }

    public void Hide() {
        gameObject.SetActive(false);
        move = false;
    }

    IEnumerator Move() {
        Debug.Log("move is " + move);

        Debug.Log("moving");
        transform.Rotate(new Vector3(0, 0, -360 / 12));

        yield return new WaitForSeconds(timeToRotate);

        if (move) {
            StartCoroutine(Move());
        }

        Debug.Log("finish");

        yield return null;
    }
}