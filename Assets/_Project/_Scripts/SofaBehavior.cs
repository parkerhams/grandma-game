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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " entered");
        if (other.tag == "Player")
        {
            if(readyToEnd)
            {
                EndGame();
            }
        }
    }

    void EndGame()
    {
        //if we want to move the player and lock them in place, fade to black first and then RepositionPlayer()

        //darken the lights in the scene
        DimScreen();
        //play ending music
        SoundManager.Instance.PlayMusic();
        //fade out video audio
        CRTscript.SetVideoVolume();
        //show credits? titlecard? show restart button
        StartCoroutine(ScrollPanelUpCoroutine());
    }

    void DimScreen()
    {
        //slowly turn the screen to a darker color, or just dim the lights in the scene
        foreach(GameObject lightGO in lights)
        {
            Light theLight = lightGO.GetComponent<Light>();
            StartCoroutine(DimLightCoroutine(theLight, .1f));
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


    IEnumerator FadeCoroutine(bool toBlack, float desiredDarkness)
    {
        yield return new WaitForSeconds(.1f);
    }

    IEnumerator ScrollPanelUpCoroutine()
    {
        while(canvas.transform.position.y < 0)
        {
            canvas.transform.Translate(Vector3.up * .07f * Time.deltaTime);
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
