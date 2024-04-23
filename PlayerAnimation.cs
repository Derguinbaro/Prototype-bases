using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator playerAnimator;

    public string currentState;
    public string playerIddle = "Iddle";
    public string playerJump = "Jump";
    public string playerRun = "Run";
    public string playerGrab = "LedgeGrab";
    public string playerHit1 = "Attack1";
    public string playerHit2 = "Attack2";
    public string playerHit3 = "Attack3";


    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(playerHit1) && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            PlayerVariables.isHitting[0] = false;
            
            if (PlayerVariables.isHitting[1])
            {
                PlayAnim(playerHit2);
            }
        }
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(playerHit2) && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            PlayerVariables.isHitting[1] = false;

            if (PlayerVariables.isHitting[2])
            {
                PlayAnim(playerHit3);
            }
        }
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(playerHit3) && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            PlayerVariables.isHitting[2] = false;
        }

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(playerJump))
        {
            PlayerVariables.isHitting[0] = false;
            PlayerVariables.isHitting[1] = false;
            PlayerVariables.isHitting[2] = false;
        }

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(playerGrab) && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            GetComponent<PlayerLedgeGrab>().ChangePos();
        }


        if (PlayerVariables.isWalking && !PlayerVariables.isJumping && !PlayerVariables.isHitting[0] && !PlayerVariables.isHitting[1] && !PlayerVariables.isHitting[2])
        {
            PlayAnim(playerRun);
        }
        else if (!PlayerVariables.isWalking && !PlayerVariables.isJumping && !PlayerVariables.isHitting[0] && !PlayerVariables.isHitting[1] && !PlayerVariables.isHitting[2])
        {
            PlayAnim(playerIddle);
        }
        else if (PlayerVariables.isJumping && !PlayerVariables.isGrabbing && !PlayerVariables.isHitting[0] && !PlayerVariables.isHitting[1] && !PlayerVariables.isHitting[2])
        {
            PlayAnim(playerJump);
        }
        else if (PlayerVariables.isGrabbing && !PlayerVariables.isHitting[0] && !PlayerVariables.isHitting[1] && !PlayerVariables.isHitting[2])
        {
            PlayAnim(playerGrab);
        }
        else if (PlayerVariables.isHitting[0] && !PlayerVariables.isHitting[1] && !PlayerVariables.isHitting[2])
        {
            PlayAnim(playerHit1);
        }
    }

    public void PlayAnim(string newState)
    {
        if (currentState == newState) return;
        playerAnimator.Play(newState);
        currentState = newState;
    }
}
