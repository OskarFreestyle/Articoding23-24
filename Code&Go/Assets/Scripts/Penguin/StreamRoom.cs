using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamRoom : MonoBehaviour {
    [SerializeField] private Animator penguinAnim;

    private Animator streamRoomAnim;

    private void Awake()
    {
        streamRoomAnim = GetComponent<Animator>();
    }

    public void FinishLevel()
    {
        penguinAnim.SetTrigger("Walk");
        streamRoomAnim.SetTrigger("Finish");
    }

    public void GameOver()
    {
        penguinAnim.SetTrigger("Walk");
        streamRoomAnim.SetTrigger("GameOver");
    }
    public void Retry()
    {
        penguinAnim.SetTrigger("Idle");
        streamRoomAnim.SetTrigger("Retry");
    }

}
