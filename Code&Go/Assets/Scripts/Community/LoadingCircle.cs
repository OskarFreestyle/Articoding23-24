using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCircle : MonoBehaviour {
    [SerializeField] private Image image;

    [SerializeField] private Color levelsColor;
    [SerializeField] private Color playlistColor;

    [SerializeField] private Button levelsLikedButtonA;
    [SerializeField] private Button levelsLikedButtonB;
    [SerializeField] private Button levelsSearchButton;
    [SerializeField] private Button levelsOrderButtonA;
    [SerializeField] private Button levelsOrderButtonB;
     
    [SerializeField] private Button playlistsLikedButtonA;
    [SerializeField] private Button playlistsLikedButtonB;
    [SerializeField] private Button playlistsSearchButton;
    [SerializeField] private Button playlistsOrderButtonA;
    [SerializeField] private Button playlistsOrderButtonB;

    [SerializeField] private float timeToRotate;
    private float timeToRotateAux;
    private bool move = false;

    public void Show(bool isLevelsColor) {
        if (isLevelsColor) image.color = levelsColor;
        else image.color = playlistColor;
        gameObject.SetActive(true);
        move = true;
        StateButtons(false);
        StartCoroutine(Move());
    }

    public void Hide() {
        gameObject.SetActive(false);
        move = false;
        StateButtons(true);
    }

    IEnumerator Move() {
        transform.Rotate(new Vector3(0, 0, -360 / 12));

        yield return new WaitForSeconds(timeToRotate);

        if (move) StartCoroutine(Move());

        yield return null;
    }

    private void StateButtons(bool state) {
        levelsLikedButtonA.interactable = state;
        levelsLikedButtonB.interactable = state;
        levelsSearchButton.interactable = state;
        levelsOrderButtonA.interactable = state;
        levelsOrderButtonB.interactable = state;
        playlistsLikedButtonA.interactable = state;
        playlistsLikedButtonB.interactable = state;
        playlistsSearchButton.interactable = state;
        playlistsOrderButtonA.interactable = state;
        playlistsOrderButtonB.interactable = state;
    }
}