﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Audio;

public class CRTBehavior : MonoBehaviour
{
    //TODO: Refactor Signal as an inheritable interface
    public enum Signal { Video, LeftAudio, RightAudio, Power, None}; //Denotes the type of signal a cable carries, including no signal
    public enum CableType { Power, RCA};

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

    [SerializeField]
    private ButtonBehavior powerButtonVCR;
    [SerializeField]
    private ButtonBehavior playButtonVCR;
    [SerializeField]
    private ButtonBehavior pauseButtonVCR;

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

    [Header("Lights")]
    [SerializeField]
    private GameObject lightCRTPower;
    [SerializeField]
    private GameObject lightVCRPower;


    [Header("Video Player")]
    //Public Reference to the CRT Screen's Video Player (could be accessed with VHS behaviors to change video being played)
    public VideoPlayer videoPlayer;
    //serialized references to the video clips playable by the CRT
    [SerializeField]
    private VideoClip blankScreenClip;
    [SerializeField]
    private VideoClip highschoolConcertClip;

    [Header("VHS Tape Objects")]
    [SerializeField]
    private GameObject BandVHSTape;

    //Variables for keeping track of the CRT's current state
    private bool isOn = false;
    private bool VCRIsOn = false;
    private bool hasPower = false;
    private bool VCRHasPower = false;

    private enum Channel { Input, Channel1, Channel2};

    [Header("CRT State")]
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

    AudioSource audioSource;

    public AudioSource musicSource;
    float musicVolume;


    private TriggerDialogue crtDialogueTrigger;



    // Start is called before the first frame update
    void Start()
    {
        crtDialogueTrigger = this.GetComponent<TriggerDialogue>();
        musicVolume = musicSource.volume;
        videoPlayer.loopPointReached += EndReached;

        if (!GetComponent<AudioSource>())
        {
            Debug.Log("No audio source component on " + gameObject.name + "! It needs one!");
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void EndReached(VideoPlayer vp)
    {
        //vp.playbackSpeed = 1;
        vp.Stop();
        vp.Play();
    }


    // Update is called once per frame
    void Update()
    {
        CheckPower(); 
        CheckButtons();
        UpdateScreenState();
        UpdateDebugText();
    }

    public void DebugButtonInfoUpdate(string newText)
    {
        debugButtonInfo.text = newText;
    }

    void ResumeMusic()
    {
        musicSource.volume = musicVolume;
    }

    private void UpdateDebugText()
    {
        debugTextPower.text = "CRT Power: " + powerSocket.signal.ToString();
        debugTextLeftAudio.text = "Left audio: " + leftAudioSocket.signal.ToString();
        debugTextVideo.text = "Video: " + videoSocket.signal.ToString();
        debugTextRightAudio.text = "Right audio: " + rightAudioSocket.signal.ToString();
        debugVCRPower.text = "VCR Power: " + powerVCRSocket.signal.ToString();
    }

    void PlayIfNotPlaying()
    {
        if (!videoPlayer.isPlaying && !videoPlayer.isPaused)
        {
            videoPlayer.Play();
            musicSource.volume = 0;
            //videoPlayer.playbackSpeed = 1;
        }
    }
    
    void UpdateScreenState() //Modifies the screen state (TODO: Implement video player into Channel Behavior)
    {
        if (hasPower && isOn)
        {
            

            switch (currentChannel)
            {
                case Channel.Input:
                    ChannelText.text = "INPUT";
                    UpdateInputChannel();
                    break;
                case Channel.Channel1:
                    ChannelText.text = "CHANNEL-1";
                    //show blank screen
                    if (videoPlayer.clip != blankScreenClip) //sees if the correct clip is already loaded, if not, changes and plays the clip
                    {
                        videoPlayer.Stop();
                        videoPlayer.clip = blankScreenClip;
                        PlayIfNotPlaying();
                    }
                    PlayIfNotPlaying();
                    break;
                case Channel.Channel2:
                    ChannelText.text = "CHANNEL-2";
                    //show blank screen
                    if (videoPlayer.clip != blankScreenClip) //sees if the correct clip is already loaded, if not, changes and plays the clip
                    {
                        videoPlayer.Stop();
                        videoPlayer.clip = blankScreenClip;
                        PlayIfNotPlaying();
                    }
                    PlayIfNotPlaying();
                    break;
                default:
                    break;
            }
        }
        else
        {
            ChannelText.text = null;
            videoPlayer.Stop(); //stops the video if there is no power
            ResumeMusic();
        }
    }

    void UpdateInputChannel()
    {
        //check VCR first
        if(!VCRIsOn || !VCRHasPower)//if the VCR doesn't have the proper conditions to play video
        {
            if (videoPlayer.clip != blankScreenClip) //sees if the correct clip is already loaded, if not, changes and plays the clip
            {
                videoPlayer.Stop();
                videoPlayer.clip = blankScreenClip;
                PlayIfNotPlaying();
            }
            PlayIfNotPlaying();
        }
        //Video Socket Logic
        else if (videoSocket.signal == SocketBehavior.Signal.Video && currentVHS)
        {
            ChannelText.text = "PLAY";
            //Play the Video
            if (currentVHS == BandVHSTape)//we can add info inside VHSBehavior and reference that instead of using the name if we want
            {
                //play video 1
                if (videoPlayer.clip != highschoolConcertClip) //sees if the correct clip is already loaded, if not, plays the clip
                {
                    videoPlayer.Stop();
                    videoPlayer.clip = highschoolConcertClip;
                    PlayIfNotPlaying();
                }
                PlayIfNotPlaying();
            }
        }
        else if (videoSocket.signal == SocketBehavior.Signal.Video && !currentVHS)
        {
            //play video 1
            if (videoPlayer.clip != blankScreenClip) //sees if the correct clip is already loaded, if not, changes and plays the clip
            {
                videoPlayer.Stop();
                videoPlayer.clip = blankScreenClip;
                PlayIfNotPlaying();
            }
            PlayIfNotPlaying();
        }
        else if (videoSocket.signal == SocketBehavior.Signal.None)
        {
            //play video 1
            if (videoPlayer.clip != blankScreenClip) //sees if the correct clip is already loaded, if not, changes and plays the clip
            {
                videoPlayer.Stop();
                videoPlayer.clip = blankScreenClip;
                PlayIfNotPlaying();
            }
            PlayIfNotPlaying();
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

    void CheckPower()
    {
        if (powerSocket.signal == SocketBehavior.Signal.Power) //if the power cable is plugged 
        {
            hasPower = true; //set the power state on
            if(isOn)
            {
                ToggleLight(lightCRTPower, true);
                PixelCrushers.DialogueSystem.DialogueManager.StartConversation("GameStartDialogue");
            }
        }
        else //the cable is unplugged, or has been unplugged
        {
            hasPower = false; //set it to false
            ToggleLight(lightCRTPower, false);
        }

        if (powerVCRSocket.signal == SocketBehavior.Signal.Power) //if the VCR power cable is plugged
        {
            VCRHasPower = true;
            if (VCRIsOn)
            {
                ToggleLight(lightVCRPower, true);
            }
        }
        else
        {
            VCRHasPower = false;
            ToggleLight(lightVCRPower, false);
        }
    }

    void ToggleLight(GameObject lightSource, bool on)
    {
        if(!lightSource)
        {
            return;
        }
        if(on)
        {
            if(!lightSource.activeSelf)
            {
                lightSource.SetActive(true);
            }
        }
        else
        {
            if (lightSource.activeSelf)
            {
                lightSource.SetActive(false);
            }
        }
    }

    public void VCRPause()
    {
        if(AllConditionsForVideo())
        {
            videoPlayer.Pause();
        }
    }

    public void VCRPlay()
    {
        if(AllConditionsForVideo())
        {
            if(!videoPlayer.isPlaying && videoPlayer.isPaused)
            {
                videoPlayer.Play();
            }
        }
    }

    bool AllConditionsForVideo()
    {
        if (hasPower && VCRHasPower && isOn && VCRIsOn && videoSocket.signal == SocketBehavior.Signal.Video && currentVHS)
        {
            return true;
        }
        else return false;
    }

    public void CheckButtons() //checks each of the buttons to see if they've been pressed
    {
        if (hasPower) //if the TV has power plugged in
        {
            //Button Press Behavior
            if (powerButton.isPressed) //if the power button is pressed
            {
                //power button will simply know whether it's pressed in or not; no resetting the variable
                //that way the button being pressed inwards will always mean it's in "on" mode, even if you press it while power isn't hooked up
                isOn = true;
            }
            else
            {
                isOn = false;
                ToggleLight(lightCRTPower, false);
            }
            if (powerButtonVCR.isPressed) //if the power button is pressed
            {
                //power button will simply know whether it's pressed in or not; no resetting the variable
                //that way the button being pressed inwards will always mean it's in "on" mode, even if you press it while power isn't hooked up
                VCRIsOn = true;
            }
            else
            {
                VCRIsOn = false;
                ToggleLight(lightVCRPower, false);
            }

            if(pauseButtonVCR.isPressed)
            {
                pauseButtonVCR.isPressed = false;
                VCRPause();
            }

            if (playButtonVCR.isPressed)
            {
                playButtonVCR.isPressed = false;
                VCRPlay();
            }

            if (channelUpButton.isPressed) //if the Channel Up Button is Pressed
            {
                channelUpButton.isPressed = false; //Reset the button's state
                if (isOn) //check to see if that TV is on
                {
                    currentChannel++; //Turn the channel up
                    videoPlayer.Stop();
                    if (currentChannel >= Channel.Channel2)
                    {
                        currentChannel = Channel.Channel2;
                    }
                }
            }
            if (channelDownButton.isPressed) //if the Channel Up Button is Pressed
            {
                channelDownButton.isPressed = false; //Reset the button's state
                if (isOn) //check to see if that TV is on
                {
                    currentChannel--; //Turn the channel down
                    if (currentChannel <= Channel.Input)
                    {
                        currentChannel = Channel.Input;
                        videoPlayer.Stop();
                    }
                }
            }
        }
    }
}
