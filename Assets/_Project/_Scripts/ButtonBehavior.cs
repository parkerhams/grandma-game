using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    [HideInInspector]
    public bool isPressed; // variable to check whether the button has been pressed

    //isPressed is set back to false when its corresponding function is called in the device's script to ensure that the device "hears" when it is pressed

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHand") //if a player hand enters the button trigger
        {
            isPressed = true; //set isPressed to true
        }
    }
}
