﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CRTBehavior : MonoBehaviour
{
    //TODO: Refactor Signal as an inheritable interface
    public enum Signal { Video, LeftAudio, RightAudio, Power, None }; //Denotes the type of signal a cable carries, including no signal
    public enum CableType { Power, RCA };

    //The VHS currently in the VCR. This is handled and updated by the VCR
    [HideInInspector]
    public GameObject currentVHS;

    [Header("Buttons")]
    //Serialized references to the Device's Buttons
    [SerializeField]
    private ButtonBehavior powerButton;
    [SerializeField]
    private ButtonBehavior channelUpButton;
    [SerializeField]
    private ButtonBehavior channelDownButton;

    [Header("Sockets")]
    //Serialized references to the Device's Sockets
    [SerializeField]
    private SocketBehavior powerSocket;
    [SerializeField]
    private SocketBehavior videoSocket;
    [SerializeField]
    private SocketBehavior leftAudioSocket;
    [SerializeField]
    private SocketBehavior rightAudioSocket;
    [SerializeField]
    private SocketBehavior powerVCRSocket;


    [Header("Video Player")]
    //Public Reference to the CRT Screen's Video Player (could be accessed with VHS behaviors to change video being played)
    public VideoPlayer videoPlayer;
    //serialized references to the video clips playable by the CRT
    [SerializeField]
    private VideoClip blankScreenClip;
    [SerializeField]
    private VideoClip highschoolConcertClip;

    [Header("CRT State")]
    //Variables for keeping track of the CRT's current state
    private bool isOn = false;
    private bool hasPower = false;

    private enum Channel { Input, Channel1, Channel2 };
    [SerializeField]
    private Channel currentChannel = Channel.Channel2;
    [SerializeField]
    private Text ChannelText; //text element for printing the channel over the screen

    [Header("Debug Text")]
    //for printing debug log messages about status to CRT canvas. removed when testing is done
    public Text debugTextPower;
    public Text debugVCRPower;
    public Text debugTextLeftAudio;
    public Text debugTextVideo;
    public Text debugTextRightAudio;
    public Text debugButtonInfo;

    [Header("Acceptable VHS Objects")]
    [SerializeField]
    private GameObject BandVHSTape;

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
        updateDebugText();
    }

    public void debugButtonInfoUpdate(string newText)
    {
        debugButtonInfo.text = newText;
    }

    void updateDebugText() //updates text on the debug canvas
    {
        debugTextPower.text = "CRT Power: " + powerSocket.signal.ToString();
        debugTextLeftAudio.text = "Left audio: " + leftAudioSocket.signal.ToString();
        debugTextVideo.text = "Video: " + videoSocket.signal.ToString();
        debugTextRightAudio.text = "Right audio: " + rightAudioSocket.signal.ToString();
        debugVCRPower.text = "VCR Power: " + powerVCRSocket.signal.ToString();
    }

    void updateScreenState() //updates what the screen is doing based on power, channel, and input
    {
        if (hasPower && isOn)
        {
            switch (currentChannel)
            {
                case Channel.Input:
                    ChannelText.text = "INPUT";
                    updateInputChannel();
                    break;
                case Channel.Channel1:
                    ChannelText.text = "CHANNEL-1";
                    //show blank screen
                    if (videoPlayer.clip != blankScreenClip) //sees if the correct clip is already loaded, if not, changes and plays the clip
                    {
                        videoPlayer.Stop();
                        videoPlayer.clip = blankScreenClip;
                        videoPlayer.Play();
                    }
                    break;
                case Channel.Channel2:
                    ChannelText.text = "CHANNEL-2";
                    //show blank screen
                    if (videoPlayer.clip != blankScreenClip) //sees if the correct clip is already loaded, if not, changes and plays the clip
                    {
                        videoPlayer.Stop();
                        videoPlayer.clip = blankScreenClip;
                        videoPlayer.Play();
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            ChannelText.text = null;
        }
    }
    void updateInputChannel() //updates what video clip is showing on the Input Channel
    {
        //Video Socket Logic
        if (videoSocket.signal == SocketBehavior.Signal.Video && currentVHS)
        {
            //Play the Video
            if (currentVHS == BandVHSTape) //we can add info inside VHSBehavior and reference that instead of using the name if we want
            {
                //play video 1
                if (videoPlayer.clip != highschoolConcertClip) //sees if the correct clips is already loaded, if not, plays the clip
                {
                    videoPlayer.Stop();
                    videoPlayer.clip = highschoolConcertClip;
                    videoPlayer.Play();
                }
            }
            Debug.Log("The Television is showing the video");
        }
        else if (videoSocket.signal == SocketBehavior.Signal.Video && !currentVHS)
        {
            //play video 1
            if (videoPlayer.clip != blankScreenClip) //sees if the correct clip is already loaded, if not, changes and plays the clip
            {
                videoPlayer.Stop();
                videoPlayer.clip = blankScreenClip;
                videoPlayer.Play();
            }
        }
        else if (videoSocket.signal == SocketBehavior.Signal.None)
        {
            //play video 1
            if (videoPlayer.clip != blankScreenClip) //sees if the correct clip is already loaded, if not, changes and plays the clip
            {
                videoPlayer.Stop();
                videoPlayer.clip = blankScreenClip;
                videoPlayer.Play();
            }
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

    void checkPower() //checks whether the CRT power cable is plugged in
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

    public void checkButtons() //checks each of the buttons to see if they've been pressed
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
