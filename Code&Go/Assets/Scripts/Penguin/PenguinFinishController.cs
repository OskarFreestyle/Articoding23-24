using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinFinishController : MonoBehaviour {

    [SerializeField] private Animator animator;

    public void SetFinishAnimation(int numberStars) {

        animator.SetInteger("NumberStars", numberStars);
    }
}
