using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator rtAnimator;
    Movement rtMovement;

    void Start()
    {
        rtAnimator = GetComponent<Animator>();
        rtMovement = this.GetComponent<Movement>();
    }

    void Update()
    {
        if (rtMovement.state == Movement.PlayerStates.Sliding)
        {
            rtAnimator.SetBool("isSliding", true);
        }

        else
        {
            rtAnimator.SetBool("isSliding", false);
        }
    }
}
