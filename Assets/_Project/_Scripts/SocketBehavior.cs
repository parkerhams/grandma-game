using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketBehavior : MonoBehaviour
{

    public bool isMale = true;//a male connection has prongs and fits a female plug. if set to false, it's assumed to be female

    public bool isOccupied = false;//becomes occupied when a plug slots into the socket

    public GameObject DesiredPlugLocation;//the exact spot the plug jumps to when plugging into the socket. the plug will set its location to this game object's location

    public enum WhatSocketNeeds { Audio, Video, Power }//the type of cable this socket needs to perform its function
    public WhatSocketNeeds whatThisSocketNeeds;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Plug")
        {
            Debug.Log("A");
            //check if socket has its script
            if (!collision.gameObject.GetComponent<PlugBehavior>())
            {
                Debug.Log("Plug has no PlugBehavior script.");
                return;
            }
            //SocketBehavior socketBehaviorScript = collision.gameObject.GetComponent<SocketBehavior>();
            PlugBehavior plugBehaviorScript = collision.gameObject.GetComponent<PlugBehavior>();
            //check if socket is already occupied or plug is already plugged into something
            if (this.isOccupied || plugBehaviorScript.isPluggedIn)
            {
                return;
            }
            Debug.Log("B");
            //check if the plug and socket have incompatible genders (male + male or female + female)
            if ((plugBehaviorScript.isMale && this.isMale) || (!plugBehaviorScript.isMale && !this.isMale))
            {
                return;
            }
            //plug it in. this currently doesn't check for the connection type; we need to decide what's allowed to be incorrectly plugged where
            plugBehaviorScript.PlugIntoSocket(this);
        }
    }

    public void ReceivePlug()
    {
        //all compatibility checks and information broadcasting are performed in PlugBehavior
        isOccupied = true;
    }
    public void RemovePlug()
    {
        isOccupied = false;
    }
}
