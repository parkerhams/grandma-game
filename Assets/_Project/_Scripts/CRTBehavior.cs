using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CRTBehavior : MonoBehaviour
{
    //TODO: Refactor Signal as an inheritable interface
    public enum Signal { Video, LeftAudio, RightAudio, Power, None}; //Denotes the type of signal a cable carries, including no signal
    public enum CableType { Power, RCA};

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
    private enum Channel { Input, Channel1, Channel2};
    [SerializeField]
    private Channel currentChannel = Channel.Channel2;

    //for printing debug log messages about status to CRT canvas. removed when testing is done
    public Text debugTextPower;
    public Text debugTextLeftAudio;
    public Text debugTextVideo;
    public Text debugTextRightAudio;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkPower();
        checkButtons();
        updateScreenState();
        debugTextPower.text = "Power: " + powerSocket.signal.ToString();
        debugTextLeftAudio.text = "Left audio: " + leftAudioSocket.signal.ToString();
        debugTextVideo.text = "Video: " + videoSocket.signal.ToString();
        debugTextRightAudio.text = "Right audio: " + rightAudioSocket.signal.ToString();
    }
    
    void updateScreenState() //Modifies the screen state (TODO: Implement video player into Channel Behavior)
    {
        if (hasPower && isOn)
        {
            switch (currentChannel)
            {
                case Channel.Input:
                    updateInputChannel();
                    break;
                case Channel.Channel1:
                    //Display Static with channel number
                    break;
                case Channel.Channel2:
                    //Display Static with channel number
                    break;
                default:
                    break;
            }
        }
    }
    void updateInputChannel()
    {
        //Video Socket Logic
        if (videoSocket.signal == SocketBehavior.Signal.Video)
        {
            //Play the Video
            Debug.Log("The Television is showing the video");
        }
        else if (videoSocket.signal == SocketBehavior.Signal.None)
        {
            //Display Blank Input Screen (Bluescreen w/ VCR "INPUT" Title
            Debug.Log("The Television is showing Nothing");
        }
        else //Audio has been plugged into the video socket
        {
            //The screen shows static
            Debug.Log("The Television is showing static [An audio cable is plugged into the video port");
        }

        //Audio Socket Logic
        //If the audio is connected correctly
        if (leftAudioSocket.signal == SocketBehavior.Signal.LeftAudio && rightAudioSocket.signal == SocketBehavior.Signal.RightAudio)
        {
            //Play the video audio
            Debug.Log("The Audio is playing correctly");
        }
        //If the audio signal is switched
        else if (leftAudioSocket.signal == SocketBehavior.Signal.RightAudio && rightAudioSocket.signal == SocketBehavior.Signal.LeftAudio)
        {
            //Play the audio backwards
            Debug.Log("The Audio plays backwards");
        }
        //If only one audio socket is plugged in
        else if (leftAudioSocket.signal == SocketBehavior.Signal.LeftAudio || rightAudioSocket.signal == SocketBehavior.Signal.RightAudio)
        {
            //Play audio at half volume / with weird artifacting / not quite right
        }
        //if the audio socket has a video signal, or no signal
        else
        {
            //No Audio Plays
        }
    }

    void checkPower()
    {
        if (powerSocket.signal == SocketBehavior.Signal.Power) //if the power cable is plugged 
        {
            hasPower = true; //set the power state on
        }
        else //the cable is unplugged, or has been unplugged
        {
            hasPower = false; //set it to false
        }
    }

    void checkButtons() //checks each of the buttons to see if they've been pressed
    {
        if (hasPower) //if the TV has power plugged in
        {
            //Button Press Behavior
            if (powerButton.isPressed) //if the power button is pressed
            {
                powerButton.isPressed = false; //Reset the button's state

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
                    currentChannel++; //Turn the channel up
                }
            }
            if (channelDownButton.isPressed) //if the Channel Up Button is Pressed
            {
                channelDownButton.isPressed = false; //Reset the button's state
                if (isOn) //check to see if that TV is on
                {
                    currentChannel--; //Turn the channel down
                }
            }
        }
    }
}
