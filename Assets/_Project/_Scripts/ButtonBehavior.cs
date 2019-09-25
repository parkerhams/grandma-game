﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    [HideInInspector]
    public bool isPressed; // variable to check whether the button has been pressed. CRTBehavior resets this after checking it!
    public bool sticksInWhenPressed = false;//this type of button will lock into a closer position when pressed in, like a pen. used for the power buttons
    public bool isPressedInwards = false;//power button is considered pressed inwards when locked into its closer position
    public float pressedInDistance = .02f;//how far in the button moves when pressed

    public CRTBehavior CRTBehaviorScript;

    bool canBePressed = true;

    public float waitTime = 1f;//time to wait, in seconds, until button can be interacted with again

    //isPressed is set back to false when its corresponding function is called in the device's script to ensure that the device "hears" when it is pressed

    private void OnTriggerEnter(Collider other)
    {
        CRTBehaviorScript.debugButtonInfoUpdate(other.gameObject.name + ", " + other.gameObject.tag);
        if (other.tag == "DistanceGrabbable" || (other.gameObject.name == "DistanceGrabHandLeft Variant" || other.gameObject.name == "DistanceGrabHandRight Variant") ) //if a player hand enters the button trigger
        {
            CRTBehaviorScript.debugButtonInfoUpdate("2");
            if (canBePressed)
            {
                CRTBehaviorScript.debugButtonInfoUpdate("3");
                PressButton();
            }
        }
    }

    void UnpressPowerButton()
    {

    }

    void PressButton()
    {
        CRTBehaviorScript.debugButtonInfoUpdate("A");
        if(sticksInWhenPressed)//if it's a power button, behavior should be slightly different: lock into place when pressed in, or unlock when pressed again
        {
            CRTBehaviorScript.debugButtonInfoUpdate("B");
            if (isPressedInwards)
            {
                //pull button back out to normal position
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - pressedInDistance);
                isPressedInwards = false;
                CRTBehaviorScript.checkButtons();
                StartCoroutine(AfterPressWaitCoroutine());
            }
            else
            {
                CRTBehaviorScript.debugButtonInfoUpdate("C");
                //push button into its closer position and lock it there
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + pressedInDistance);
                isPressedInwards = true;
                CRTBehaviorScript.checkButtons();
                StartCoroutine(AfterPressWaitCoroutine());
            }
        }
    }

    IEnumerator AfterPressWaitCoroutine()
    {
        canBePressed = false;
        yield return new WaitForSeconds(waitTime);
        canBePressed = true;
    }
}
