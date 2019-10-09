using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableBehavior : MonoBehaviour
{
    /// <summary>
    /// Has its two plugs as game objects. When either plug sockets into something successfully, that plug tells this script.
    /// This script looks at what the two plugs are plugged into, and decides if it's carrying a signal. It carries a signal if one plug is in an input socket and the other is in a signal socket.
    /// It then tells the inputSocket (assigned whenever a plug plugs into a socket that says it's the input socket) what signal is being carried.
    /// If either plug is unplugged, this script tells the inputSocket that no signal is now being provided.
    /// </summary>
    /// 
    public GameObject plug1;//located on capsule 1
    public GameObject plug2;//located on the highest number capsule

    PlugBehavior plug1Behavior;
    PlugBehavior plug2Behavior;
    SocketBehavior socket1Behavior;
    SocketBehavior socket2Behavior;

    SocketBehavior inputSocket;
    SocketBehavior signalSocket;

    private void Start()
    {
        plug1Behavior = plug1.GetComponent<PlugBehavior>();
        plug2Behavior = plug2.GetComponent<PlugBehavior>();
    }

    public void UpdatePlugStatus(GameObject whichPlug)//called whenever one of its plugs plugs into a socket
    {
        //assign socket behavior scripts
        if(whichPlug == plug1)
        {
            if (plug1Behavior.currentSocketBehaviorScript)
            {
                socket1Behavior = plug1Behavior.currentSocketBehaviorScript;
                if (socket1Behavior.isInputSocket)
                {
                    inputSocket = socket1Behavior;
                }
            }
        }
        if(whichPlug == plug2)
        {
            if (plug2Behavior.currentSocketBehaviorScript)
            {
                socket2Behavior = plug2Behavior.currentSocketBehaviorScript;
                if(!socket1Behavior)
                {
                    return;
                }
                if (socket1Behavior.isInputSocket)
                {
                    inputSocket = socket1Behavior;
                }
            }
        }

        //determine which socket is an input socket and which is a signal socket
        DetermineSocketTypes();

        //Tell the input socket what signal it's receiving
        CommunicateSignalStatus(true);
    }

    public void CommunicateSignalStatus(bool hasSignal)//updates the input socket with what signal it's receiving, if any
    {
        if(!hasSignal)//this argument is set false if it's determined that there's about to be no input socket and we should set the signal to None before the input socket is removed
        {
            if(!inputSocket)
            {
                Debug.Log("Tried to terminate signal status on the socket that " + gameObject + " was connected to, but inputSocket was null before signal could be terminated.");
                return;
            }
            inputSocket.signal = SocketBehavior.Signal.None;
            return;
        }
        
        if(!inputSocket || !signalSocket)//if this function was called in UpdatePlugStatus, it won't know if there's a valid input or signal socket, so check here before continuing
        {
            return;
        }

        if(inputSocket && signalSocket)//make sure it has both types of sockets
        {
            inputSocket.signal = signalSocket.signal;
        }
    }

    void DetermineSocketTypes()//checks each plug's socket to see if they're an input or signal socket, and sets the CableBehavior's inputSocket or signalSocket to null if it determines they don't exist anymore
    {
        if(socket1Behavior && socket2Behavior)//if both plugs are socketed
        {
            if(socket1Behavior.isInputSocket && socket2Behavior.isInputSocket)
            {
                //if the player dumbly plugs both plugs into input sockets, just default to #1 being the current input socket
                inputSocket = socket1Behavior;
                signalSocket = null;
            }
            else if(socket1Behavior.isInputSocket && !socket2Behavior.isInputSocket)
            {
                inputSocket = socket1Behavior;
                signalSocket = socket2Behavior;
            }
            else if(!socket1Behavior.isInputSocket && socket2Behavior.isInputSocket)
            {
                inputSocket = socket2Behavior;
                signalSocket = socket1Behavior;
            }
            else//if the player dumbly plugs both plugs into signal sockets, just default to #1 being the current signal socket
            {
                CommunicateSignalStatus(false);
                inputSocket = null;
                signalSocket = socket1Behavior;
            }
        }

        else if(socket1Behavior && !socket2Behavior)//if only plug 1 is socketed
        {
            if(socket1Behavior.isInputSocket)
            {
                inputSocket = socket1Behavior;
                signalSocket = null;
            }
            else
            {
                CommunicateSignalStatus(false);
                inputSocket = null;
                signalSocket = socket1Behavior;
            }
        }

        else if(socket2Behavior && !socket1Behavior)//if only plug 2 is socketed
        {
            if (socket2Behavior.isInputSocket)
            {
                inputSocket = socket2Behavior;
                signalSocket = null;
            }
            else
            {
                CommunicateSignalStatus(false);
                inputSocket = null;
                signalSocket = socket2Behavior;
            }
        }
        else//if neither plug is socketed
        {
            //there is no input socket
            inputSocket = null;
            signalSocket = null;
        }
    }

    public void TerminateSignalStatus()//called whenever a plug unplugs from a socket
    {
        if (!plug1Behavior.currentSocketBehaviorScript)
        {
            socket1Behavior = null;
        }
        if (!plug2Behavior.currentSocketBehaviorScript)
        {
            socket2Behavior = null;
        }
        DetermineSocketTypes();
        CommunicateSignalStatus(false);
    }
}
