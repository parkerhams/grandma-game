using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SofaBehavior : MonoBehaviour
{
    public bool readyToEnd = false;
    public GameObject endingPosition;
    public CRTBehavior CRTscript;
    public GameObject canvas;

    public GameObject[] lights;
    public GameObject restartButton;

    bool hasEnded = false;//once the ending is started, it can not be triggered any more

    float canvasEndingYPosition = .5f;//how high up the canvas scrolls until it stops
    float lightIntensityWhenDimmed = .06f;//how dark the lights get when the game ends
    float scrollSpeed = .09f;//how quickly the canvas scrolls up
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(readyToEnd && !hasEnded)
            {
                EndGame();
            }
        }
    }

    void EndGame()
    {
        hasEnded = true;

        //prevent the end button from being pressable for a short moment after the ending begins
        StartCoroutine(restartButton.GetComponent<ButtonBehavior>().AfterPressWaitCoroutine(3));
        //if we want to move the player and lock them in place, fade to black first and then RepositionPlayer()

        //darken the lights in the scene
        DimScreen();
        //play ending music
        SoundManager.Instance.PlayMusic();
        //fade out video audio
        CRTscript.videoVolumeOverwritten = true;
        CRTscript.SetVideoVolume();
        //show credits? titlecard? show restart button
        StartCoroutine(ScrollPanelUpCoroutine());
        restartButton.SetActive(true);

        //dialogue
        if(CRTscript.santaVideoPlaying)
        {
            //dialogue call: reminisce about santa video during end of game
            DialogueManager.Instance.Bark(DialogueManager.Instance.santaClipIsPlayingAtEnd);
        }
        else if(CRTscript.concertVideoPlaying)
        {
            //dialogue call: reminisce about concert video during end of game
            DialogueManager.Instance.Bark(DialogueManager.Instance.bandClipIsPlayingAtEnd);
        }
    }

    void DimScreen()
    {
        //slowly turn the screen to a darker color, or just dim the lights in the scene
        foreach(GameObject lightGO in lights)
        {
            Light theLight = lightGO.GetComponent<Light>();
            StartCoroutine(DimLightCoroutine(theLight, lightIntensityWhenDimmed));
        }
    }

    /// <summary>
    /// Not sure if we want to actually reposition the player onto the couch and lock them in place.
    /// If not, we'll just not use this function.
    /// If we do, we'll probably need to fade to black very quickly before moving them to avoid disorientation.
    /// </summary>
    /// <param name="player"></param>
    void RepositionPlayer(GameObject player)
    {
        //put player in ending position
        player.transform.position = endingPosition.transform.position;
        //prevent movement via analog (or all movement?)
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        //make player start facing towards CRT
        player.transform.rotation = endingPosition.transform.rotation;
    }



    IEnumerator ScrollPanelUpCoroutine()
    {
        while(canvas.transform.position.y < canvasEndingYPosition)
        {
            canvas.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
            yield return new WaitForSeconds(.01f);
        }
    }

    IEnumerator DimLightCoroutine(Light light, float desiredIntensity)
    {
        if(desiredIntensity > .7 || desiredIntensity < 0)
        {
            desiredIntensity = .3f;//if the value is weird, make it something normal
        }
        while(light.intensity > desiredIntensity)
        {
            yield return new WaitForSeconds(.1f);
            light.intensity -= .01f;
        }
    }

}
