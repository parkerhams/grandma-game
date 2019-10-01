﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VCRBehavior : MonoBehaviour
{
    GameObject currentVHS;//the VHS that's currently inside the VCR

    public GameObject flapUp;//having two separate game objects for the flap in its different positions was easier than rotating/moving it
    public GameObject flapDown;
    public GameObject entryPosition;//where the VHS will jump to when it's getting ready to move inside the VCR
    public GameObject insidePosition;//where the VHS will be when it's fully inside the VCR

    public Text debugText;

    float waitTime = 1.5f;//when a VHS is accepted or ejected, don't accept or remove any VHS for this duration
    bool isWaiting = false;//set to true while the VHS has recently accepted or ejected a VHS

    private void Start()
    {
        flapUp.SetActive(false);//if this flap isn't disabled in scene view, disable it now
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "DistanceGrabHandLeft Variant" || other.gameObject.name == "DistanceGrabHandRight Variant") //if a player hand enters the button trigger
        {
            EjectVHS();
        }
        else if(other.GetComponent<VHSBehavior>())
        {
            AcceptVHS(other.gameObject);
        }
    }

    void MoveFlap(bool up)//the flap of the VCR that gets pushed up as you put a VHS in
    {
        if(up)
        {
            flapUp.SetActive(true);
            flapDown.SetActive(false);
        }
        else
        {
            flapUp.SetActive(false);
            flapDown.SetActive(true);
        }
    }

    void EjectVHS()
    {
        debugText.text = "eject attempted";
        if (isWaiting)
        {
            debugText.text = "still waiting";
            return;
        }
        if (!currentVHS)
        {
            //debugText.text = "no current VHS";
            return;//can't eject a VHS if there isn't one in it
        }
        debugText.text = "eject running";
        StartCoroutine(VHSWaitCoroutine());
        StartCoroutine(VHSMovementCoroutine(false));
        debugText.text = "eject ran";

    }

    void AcceptVHS(GameObject VHS)
    {
        debugText.text = "accept attempted";
        if (currentVHS)
        {
            return;//can't take a VHS if there's already one in it
        }
        if (isWaiting)
        {
            return;
        }
        debugText.text = "accept running";
        //ungrab VHS
        if (VHS.GetComponent<OVRGrabbable>().isGrabbed)
        {
            OVRGrabbable grabScript = VHS.GetComponent<OVRGrabbable>();
            grabScript.grabbedBy.ForceRelease(grabScript);
        }
        Rigidbody rb = VHS.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        VHS.transform.position = entryPosition.transform.position;
        VHS.transform.rotation = entryPosition.transform.rotation;
        StartCoroutine(VHSWaitCoroutine());
        currentVHS = VHS;
        StartCoroutine(VHSMovementCoroutine(true));
        debugText.text = "accept ran";

    }

    IEnumerator VHSMovementCoroutine(bool goingIn)
    {
        float timeWaited = 0;
        float duration = .7f;
        float step = .6f * Time.deltaTime;
        currentVHS.layer = 0;
        MoveFlap(true);
        while(timeWaited < duration)
        {
            timeWaited += Time.deltaTime;
            debugText.text = timeWaited.ToString();
            if(goingIn)
            {
                currentVHS.transform.position = Vector3.MoveTowards(currentVHS.transform.position, insidePosition.transform.position, step);
            }
            else
            {
                currentVHS.transform.position = Vector3.MoveTowards(currentVHS.transform.position, entryPosition.transform.position, step);
            }
            yield return null;
        }
        MoveFlap(false);
        if(goingIn)
        {
            currentVHS.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            currentVHS.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            //re-enable grabbing of VHS
            currentVHS.layer = 8;
            Rigidbody rb = currentVHS.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            debugText.text = currentVHS + " should be grabbable";
            currentVHS = null;
        }
    }

    IEnumerator VHSWaitCoroutine()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }
}
