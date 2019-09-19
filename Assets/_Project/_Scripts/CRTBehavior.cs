using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRTBehavior : MonoBehaviour
{
    //TODO: Refactor Signal as an inheritable interface
    public enum Signal { Video, LeftAudio, RightAudio, Power, None}; //Denotes the type of signal a cable carries, including no signal
    private Signal thisSignal = Signal.None;

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

    //Variables for keeping track of the CRT's current state
    private bool isOn = false;
    private bool hasPower = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkPower();
        checkButtons();
        updateRCASockets();
    }

    void checkPower()
    {
        //If the power socket is set to Signal.Power, set hasPower to true
    }

    void updateRCASockets()
    {
        if (isOn && hasPower)
        {
            //Set the left audio socket signal to Signal.LeftAudio
            //Set the right audio socket signal to Signal.RightAudio
            //Set the video socket signal to Signal.Video
        }
        else
        {
            //set all RCA sockets signals to None
        }
    }

    void checkButtons() //checks each of the buttons to see if they've been presse
    {
        if (hasPower) //if the TV has power plugged in
        {
            //Button Press Behavior
            if (powerButton.isPressed) //if the power button is pressed
            {
                powerButton.isPressed = false; //Reset the button's state

                //TODO: Refactor this into its own function?
                //On button Press, toggle power on & off
                if (isOn == false)
                {
                    isOn = true;
                }
                else
                {
                    isOn = false;
                }
            }
            if (channelUpButton.isPressed) //if the Channel Up Button is Pressed
            {
                channelUpButton.isPressed = false; //Reset the button's state
                if (isOn) //check to see if that TV is on
                {
                    //TODO: Turn the channel up
                }
            }
            if (channelDownButton.isPressed) //if the Channel Up Button is Pressed
            {
                channelDownButton.isPressed = false; //Reset the button's state
                if (isOn) //check to see if that TV is on
                {
                    //TODO: Turn the channel down
                }
            }
        }
    }
}
