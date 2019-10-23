using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    [HideInInspector]
    public bool isPressed; // variable to check whether the button has been pressed. CRTBehavior resets this after checking it!
    public bool sticksInWhenPressed = false;//this type of button will lock into a closer position when pressed in, like a pen. used for the power buttons
    public bool isPressedInwards = false;//power button is considered pressed inwards when locked into its closer position
    public float pressedInDistance = .02f;//how far in the button moves when pressed

    public bool isEjectButton = false;
    public VCRBehavior VCRscript;

    public CRTBehavior CRTBehaviorScript;

    public TriggerDialogue buttonTriggerDialogue;
    bool canBePressed = true;

    AudioSource audioSource;

    private float waitTime = .4f;//time to wait, in seconds, until button can be interacted with again

    //isPressed is set back to false when its corresponding function is called in the device's script to ensure that the device "hears" when it is pressed

    private void Start()
    {
        buttonTriggerDialogue = this.GetComponent<TriggerDialogue>();
        if(!GetComponent<AudioSource>())
        {
            Debug.Log("No audio source component on " + gameObject.name + "! It needs one!");
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CRTBehaviorScript.DebugButtonInfoUpdate(other.gameObject.name + ", " + other.gameObject.tag);
        if (other.tag == "DistanceGrabbable" || (other.gameObject.name == "DistanceGrabHandLeft Variant" || other.gameObject.name == "DistanceGrabHandRight Variant") ) //if a player hand enters the button trigger
        {
            CRTBehaviorScript.DebugButtonInfoUpdate("2");
            if (canBePressed)
            {
                CRTBehaviorScript.DebugButtonInfoUpdate("3");
                PressButton();
            }
        }
    }

    void PressButton()
    {
        if(sticksInWhenPressed)//if it's a power button, behavior should be slightly different: lock into place when pressed in, or unlock when pressed again
        {
            if (isPressedInwards)
            {
                //pull button back out to normal position
                if(!isEjectButton)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - pressedInDistance);
                }
                if(isEjectButton)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - pressedInDistance, transform.position.z);
                }
                isPressedInwards = false;
                isPressed = false;

                //AUDIO: sticky CRT button pushed in
                audioSource.PlayOneShot(SoundManager.Instance.tv_button_on);

                StartCoroutine(AfterPressWaitCoroutine());
            }
            else
            {
                //push button into its closer position and lock it there
                if (!isEjectButton)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + pressedInDistance);
                }
                if (isEjectButton)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - pressedInDistance, transform.position.z);
                }
                isPressedInwards = true;
                isPressed = true;

                //AUDIO: sticky CRT button pushed back out
                audioSource.PlayOneShot(SoundManager.Instance.tv_button_off);

                StartCoroutine(AfterPressWaitCoroutine());
            }
        }
        else
        {
            if (canBePressed)
            {
                if (!isEjectButton)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + pressedInDistance);
                }
                if (isEjectButton)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - pressedInDistance, transform.position.z);
                }
                StartCoroutine(AfterPressWaitCoroutine());
                StartCoroutine(MoveButton());
                isPressed = true;
                if(isEjectButton)
                {
                    VCRscript.EjectVHS();
                }

                //AUDIO: regular button pressed
                audioSource.PlayOneShot(SoundManager.Instance.buttonPress);
            }
        }
    }

    IEnumerator MoveButton()
    {
        float timeWaited = 0;
        float waitIncrement = .01f;
        while(timeWaited <= waitTime)
        {
            timeWaited += waitIncrement;
            //move the button a fraction of the distance back towards its normal position.
            var posIncrement = pressedInDistance / (waitTime / waitIncrement);//with a waitTime of .5f and waitIncrement of .01, posIncrement is .04
            var newZPos = pressedInDistance - (posIncrement * timeWaited);
            if (!isEjectButton)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (pressedInDistance / (waitTime / waitIncrement)));
            }
            if (isEjectButton)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + (pressedInDistance / (waitTime / waitIncrement)), transform.position.z);
            }
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (pressedInDistance / (waitTime / waitIncrement)));
            yield return new WaitForSeconds(.01f);

        }
        
    }

    IEnumerator AfterPressWaitCoroutine()
    {
        canBePressed = false;
        yield return new WaitForSeconds(waitTime);
        canBePressed = true;
    }
}
