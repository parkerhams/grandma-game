﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugBehavior : MonoBehaviour
{
    public enum Signal { Video, LeftAudio, RightAudio, Power, None }; //Denotes the type of signal a cable carries, including no signal
    public enum CableType { Power, RCA }; //denotes the cable type

    public CableType cableType; //this cable's type
    public Signal signal = Signal.None; //the signal carried by this cable's signal

    public bool isPluggedIn = false;

    public bool isBackPlug = false;
    public bool canBeUnplugged = true;

    public CableBehavior cableBehaviorScript;

    [HideInInspector]
    public SocketBehavior currentSocketBehaviorScript; //once a plug plugs into a socket, that socket is saved here

    public GameObject neighborCapsule;
    public float maxDistance = 1f;//when a cable's plug's neighbor capsule gets pulled this far away, unplug the cable from that plug's socket

    bool allowedToAttemptPlugIn = true; //if this is false, plug is completely disabled from trying to plug into anything
    bool isWorkingPlug = true; //if set to false, this plug can plug into things but won't transmit any information

    float backupDistance = 2f; //the distance the plug will eject away from the socket when unplugged
    float temporaryPlugDisableTime = 1f; //the amount of time in the seconds the plug will refuse to try to plug into anything after being unplugged, to prevent accidentally jumping right back into a socket

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (!GetComponent<AudioSource>())
        {
            Debug.Log("No audio source component on " + gameObject.name + "! It needs one!");
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isPluggedIn && currentSocketBehaviorScript)
        {
            if(!canBeUnplugged)
            {
                return;//power cable can't be removed from its input socket on the CRT
            }
            DistanceBasedUnplugCheck();//check to see if it's tensioned hard enough to automatically unplug itself
            if(transform.parent.GetComponent<OVRGrabbable>().isGrabbed && isPluggedIn)
            {
                UnplugFromSocket(keepGrabbed:true);
            }
        }
    }

    public void PlugIntoSocket(SocketBehavior socketBehaviorScript)
    {
        //plug jumps into proper location. using parent because the plug's parent is the capsule that acts as the end of the cable
        transform.parent.position = socketBehaviorScript.DesiredPlugLocation.transform.position;
        transform.parent.rotation = socketBehaviorScript.DesiredPlugLocation.transform.rotation; //need to figure out if we can easily establish a rotation for DesiredPlugLocation in the scene
        if(isBackPlug)//if this is the back plug, rotate it 180 degrees when plugging in so it faces the right way
        {
            transform.parent.RotateAround(socketBehaviorScript.DesiredPlugLocation.transform.position, socketBehaviorScript.DesiredPlugLocation.transform.right, 180f);
        }
        currentSocketBehaviorScript = socketBehaviorScript;

        //AUDIO: plug in
        audioSource.PlayOneShot(SoundManager.Instance.unplug);

        //ungrab the plug's capsule
        if (transform.parent.GetComponent<OVRGrabbable>().isGrabbed)
        {
            OVRGrabbable grabScript = transform.parent.GetComponent<OVRGrabbable>();
            grabScript.grabbedBy.ForceRelease(grabScript);
        }

        //lock the plug location and try to keep cable attached
        transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        transform.parent.GetComponent<Rigidbody>().isKinematic = true;

        //broadcast information about what plug is plugged into which socket (so that the game knows if audio should be enabled, for example)
        Debug.Log(signal.ToString() + " plug plugged into " + socketBehaviorScript.signal.ToString() + " socket.");
        socketBehaviorScript.ReceivePlug();
        isPluggedIn = true;
        cableBehaviorScript.UpdatePlugStatus(gameObject);
    }
    private void UnplugFromSocket(bool keepGrabbed=false)
    {
        if(!isPluggedIn)
        {
            return;
        }
        //broadcast the plug being disabled
        Debug.Log("Unplugged.");

        //AUDIO: unplug
        audioSource.PlayOneShot(SoundManager.Instance.unplug);

        //disable plug from being allowed to plug into anything for a brief moment, remove constraints, inform socket of unplugging
        StartCoroutine(UnplugWaitCoroutine());
        transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        transform.parent.GetComponent<Rigidbody>().isKinematic = false;
        isPluggedIn = false;
        currentSocketBehaviorScript.RemovePlug();
        currentSocketBehaviorScript = null;
        cableBehaviorScript.TerminateSignalStatus();
        if(keepGrabbed)
        {
            transform.parent.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            //if(transform.parent.GetComponent<OVRGrabbable>().isGrabbed)
            //{
            //    OVRGrabbable grabScript = transform.parent.GetComponent<OVRGrabbable>();
            //    grabScript.grabbedBy.ForceRelease(grabScript);
            //}
        }
    }

    private void DistanceBasedUnplugCheck()
    {
        //if the neighbor capsule is too far away from the plug's parent capsule, unplug
        if(!neighborCapsule)
        {
            Debug.Log("No neighbor capsule specified for distance based unplug check.");
            return;
        }
        float currentDistance = Vector3.Distance(transform.parent.position, neighborCapsule.transform.position);
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
