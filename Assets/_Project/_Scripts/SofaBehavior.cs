using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SofaBehavior : MonoBehaviour
{
    public bool readyToEnd = false;
    public GameObject endingPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
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

        DimScreen();
        //play ending music
        SoundManager.Instance.PlayMusic();
        //fade out video audio
        //show credits?
        //show "restart" button
    }

    void DimScreen()
    {
        //slowly turn the screen to a darker color, or just dim the lights in the scene
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

}
