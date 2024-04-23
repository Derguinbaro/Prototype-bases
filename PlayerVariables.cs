using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVariables : MonoBehaviour
{
    public static bool isWalking;
    public static bool isJumping;
    public static bool isGrabbing;
    public static bool[] isHitting = new bool[3];
    public static float playerDamage;

    private void Update()
    {
        if (isHitting[0])
        {
            playerDamage = 5;
        }
        else if (isHitting[1])
        {
            playerDamage = 10;
        }
        else if (isHitting[2])
        {
            playerDamage = 15;
        }
    }
}
