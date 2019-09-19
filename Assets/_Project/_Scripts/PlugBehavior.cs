using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugBehavior : MonoBehaviour
{

    public bool isMale = true;//a male connection has prongs and fits into a female socket. if set to false, it's assumed to be female
    public bool isPluggedIn = false;

    public enum WhatCableCarries { Audio, Video, Power}//the type of info the cable will send along its wire
    public WhatCableCarries whatThisCableCarries;

    SocketBehavior socketBehaviorLastKnown;//once a plug plugs into a socket, that socket is saved here

    public GameObject neighorCapsule;
    public float maxDistance = 1f;

    bool allowedToAttemptPlugIn = true;//if this is false, plug is completely disabled from trying to plug into anything
    bool isWorkingPlug = true;//if set to false, this plug can plug into things but won't transmit any information

    float backupDistance = 2f;//the distance the plug will eject away from the socket when unplugged
    float temporaryPlugDisableTime = 1f;//the amount of time in the seconds the plug will refuse to try to plug into anything after being unplugged, to prevent accidentally jumping right back into a socket

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isPluggedIn && socketBehaviorLastKnown)
        {
            DistanceBasedUnplugCheck();//check to see if it's tensioned hard enough to automatically unplug itself
        }
        else
        {
            Debug.Log("can't run yet");
        }
    }

    public void PlugIntoSocket(SocketBehavior socketBehaviorScript)
    {
        //remove player's control/grabbing of object
        //plug jumps into proper location. using parent because the plug's parent is the capsule that acts as the end of the cable
        transform.parent.position = socketBehaviorScript.DesiredPlugLocation.transform.position;
        transform.parent.rotation = socketBehaviorScript.DesiredPlugLocation.transform.rotation;//need to figure out if we can easily establish a rotation for DesiredPlugLocation in the scene
        socketBehaviorLastKnown = socketBehaviorScript;
        //ungrab the plug's capsule
        if(transform.parent.GetComponent<OVRGrabbable>().isGrabbed)
        {
            OVRGrabbable grabScript = transform.parent.GetComponent<OVRGrabbable>();
            grabScript.grabbedBy.ForceRelease(grabScript);
        }
        //lock the plug location and try to keep cable attached
        transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //broadcast information about what plug is plugged into which socket (so that the game knows if audio should be enabled, for example)
        Debug.Log(whatThisCableCarries.ToString() + " plug plugged into " + socketBehaviorScript.whatThisSocketNeeds.ToString() + " socket.");
        socketBehaviorScript.ReceivePlug();
        isPluggedIn = true;
    }
    private void UnplugFromSocket()
    {
        if(!isPluggedIn)
        {
            return;
        }
        //broadcast the plug being disabled
        Debug.Log("Unplugged.");
        //disable plug from being allowed to plug into anything for a brief moment, remove constraints, inform socket of unplugging
        StartCoroutine(UnplugWaitCoroutine());
        transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        isPluggedIn = false;
        socketBehaviorLastKnown.RemovePlug();
    }

    private void DistanceBasedUnplugCheck()
    {
        Debug.Log("running");
        //if the neighbor capsule is too far away from the plug's parent capsule, unplug
        if(!neighorCapsule)
        {
            Debug.Log("No neighbor capsule specified for distance based unplug check.");
            return;
        }
        float currentDistance = Vector3.Distance(transform.parent.position, neighorCapsule.transform.position);
        Debug.Log("Distance: " + currentDistance);
        if(currentDistance >= maxDistance)
        {
            UnplugFromSocket();
        }
    }

    IEnumerator UnplugWaitCoroutine()//disables any plugging action for that plug until the set time has elapsed, then re-enables that behavior
    {
        allowedToAttemptPlugIn = false;
        yield return new WaitForSeconds(temporaryPlugDisableTime);
        allowedToAttemptPlugIn = true;
    }
}
