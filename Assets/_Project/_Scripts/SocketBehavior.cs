using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketBehavior : MonoBehaviour
{
    public enum Signal { Video, LeftAudio, RightAudio, Power, None }; //Denotes the type of signal a cable carries, including no signal
    public enum CableType { Power, RCA };

    public CableType cableType; //this cable's type
    public Signal signal = Signal.None; //the signal carried by this cable's signal

    public bool isOccupied = false;//becomes occupied when a plug slots into the socket
    public bool isInputSocket = false;

    public GameObject DesiredPlugLocation;//the exact spot the plug jumps to when plugging into the socket. the plug will set its location to this game object's location

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Plug") //the socket collides with a plug
        {
            //Debug.Log("A");
            //check if socket has its script
            if (!collision.gameObject.GetComponent<PlugBehavior>())
            {
                Debug.Log("Plug has no PlugBehavior script.");
                return;
            }

            PlugBehavior plugBehaviorScript = collision.gameObject.GetComponent<PlugBehavior>();

            //check if socket is already occupied or plug is already plugged into something
            if (this.isOccupied || plugBehaviorScript.isPluggedIn)
            {
                return;
            }
            //Debug.Log("B");

            //check if the plug and socket are the same type
            switch (cableType)
            {
                case CableType.Power: //Socket type is power
                    if (plugBehaviorScript.cableType != PlugBehavior.CableType.Power) //cable type is NOT power
                    {
                        return;
                    }
                    break;
                case CableType.RCA: //Socket type is RCA
                    if (plugBehaviorScript.cableType != PlugBehavior.CableType.RCA) //cable type is NOT RCA
                    {
                        return;
                    }
                    break;
                default:
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
