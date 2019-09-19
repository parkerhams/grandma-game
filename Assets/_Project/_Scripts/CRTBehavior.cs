using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRTBehavior : MonoBehaviour
{
    //TODO: Refactor Signal as an inheritable interface
    public enum Signal { Video, leftAudio, rightAudio, Power, None}; //type of signal a cable carries, including no signal

    //Serialized references to the Device's Buttons
    [SerializeField]
    private ButtonBehavior powerButton;
    [SerializeField]
    private ButtonBehavior channelUpButton;
        [SerializeField]
    private ButtonBehavior channelDownButton;

    //Serialized references to the Device's Sockets
    [SerializeField]
    private SocketBehavior powerSocket;
    [SerializeField]
    private SocketBehavior videoSocket;
    [SerializeField]
    private SocketBehavior leftAudioSocket;
    [SerializeField]
    private SocketBehavior rightAudioSocket;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkButtons();
    }

    void checkButtons() //checks each of the buttons to see if they've been presse
    {
        if (powerButton.isPressed)
        {
            powerButton.isPressed = false;
            //TODO: Add logic for if button is pressed
        }

        if (channelUpButton.isPressed)
        {
            channelUpButton.isPressed = false;
            //TODO: Add logic for if button is pressed
        }

        if (channelDownButton.isPressed)
        {
            channelDownButton.isPressed = false;
            //TODO: Add logic for if button is pressed
        }
    }
}
