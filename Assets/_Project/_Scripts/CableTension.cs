using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableTension : MonoBehaviour
{
    /*
     * We want this script to know: 
     * -Whether or not the cable is plugged into anything                                           [YEET]
     * -whether the cable is being grabbed or not
     * -which number capsule this is (GameObject.name)                                                [YEET]
     * -Total number of capsules on this capsule's cable                                                      [YEET]
     * 
     * So, when we detect too much distance between the gameobject.name capsule
     * and either of its nearby capsule
     * If it's plugged in then we unplug it,
     * and when we're unplugging it then we unplug to the closer side to this gripped capsule
     * 
     * if it's not pluged into anything and it gets too much tension, just ungrab
     * OR try and tell if there's two capsules being grabbed, and only ungrab one of those capusles
     */

    private float maxLengthBetweenCapsules = .3f;

    [SerializeField]
    public GameObject parentCable;

    [SerializeField]
    private GameObject maleEndCapsule, femaleEndCapsule;

    [SerializeField]
    private Vector3 cableTetherPoint;
    //find neighbor capsule and check the distance betweenthose neighbors in fixed update

    private GameObject frontNeighborCapsule, backNeighborCapsule;//front neighbor is closer to male end of the cable (male end is capsule 1)
    private float frontDistance, backDistance;

    private PlugBehavior plugBehavior;

    private List<GameObject> capsuleChildren = new List<GameObject>();

    bool cableShouldBeChecked = false;
    OVRGrabbable myGrabScript;

    int myIndex;

    private void Start()
    {
        myGrabScript = GetComponent<OVRGrabbable>();
        EstablishCapsuleList();
        EstablishNeighbors();
    }

    private void StartA()
    {
        maxLengthBetweenCapsules = .2f;

        capsuleChildren = new List<GameObject>();

        int children = parentCable.transform.childCount;
        for (int index = 0; index < children; ++index)
        {
            capsuleChildren.Add(parentCable.transform.GetChild(index).gameObject);


            //print("For loop: " + capsuleChildren[index].name);
        }

        for (int index = 0; index < capsuleChildren.Count - 1; ++index)
        {
            //we can set the front neighbor unless our index == 0
            //we can set the back neighbor if our index less than count-1

            if (index > 0)
            {
                frontNeighborCapsule = capsuleChildren[index - 1];
                //print("frontNeighbor capsule added for " + (index+1) + ", front neighbor is " + frontNeighborCapsule);
            }

            if (index <= capsuleChildren.Count-1)
            {
                backNeighborCapsule = capsuleChildren[index + 1];
                //print("backNeighbor capsule added for " + (index+1) + ", back neighbor is " + backNeighborCapsule);
            }
        }

        plugBehavior = capsuleChildren[0].transform.Find("Plug").gameObject.GetComponent<PlugBehavior>();

        //reference capsuleCHildren[index].name for name
        //plugBehavior.isPluggedIn       

        maleEndCapsule = capsuleChildren[0];
        femaleEndCapsule = capsuleChildren[capsuleChildren.Count - 1];
    }

    void EstablishCapsuleList()
    {
        foreach(Transform child in transform.parent)
        {
            capsuleChildren.Add(child.gameObject);
            if(child.gameObject == this.gameObject)
            {
                myIndex = capsuleChildren.IndexOf(this.gameObject);
            }
        }
    }

    void EstablishNeighbors()
    {
        if(myIndex > 0)
        {
            frontNeighborCapsule = capsuleChildren[myIndex - 1];
            Debug.Log(gameObject + " front neighbor: " + frontNeighborCapsule);
        }
        int possibleLastIndex = myIndex + 1;
        if(capsuleChildren.Count > possibleLastIndex)
        {
            backNeighborCapsule = capsuleChildren[myIndex + 1];
            Debug.Log(gameObject + " back neighbor: " + backNeighborCapsule);
        }
    }

    private void Update()
    {
        if(cableShouldBeChecked)
        {
            CheckDistanceBetweenCapsules();
        }
        else
        {
            if (myGrabScript.isGrabbed)
            {
                foreach(GameObject capsule in capsuleChildren)
                {
                    capsule.GetComponent<CableTension>().cableShouldBeChecked = true;
                }
            }
            else if(cableShouldBeChecked == true)
            {
                foreach (GameObject capsule in capsuleChildren)
                {
                    capsule.GetComponent<CableTension>().cableShouldBeChecked = false;
                }
            }
        }
    }

    private void FixedUpdate()//not sure if this would work better as Update or FixedUpdate. either way it might be really expensive and we might need to only do this distance check somewhere else (when moved?)
    {

    }

    private void UngrabCable()
    {
        foreach (GameObject capsule in capsuleChildren)//look through all the capsules on this cable and ungrab them if they're being held
        {
            OVRGrabbable grabScript = capsule.GetComponent<OVRGrabbable>();
            if (grabScript.isGrabbed)
            {
                grabScript.grabbedBy.GetComponent<OVRGrabber>().ForceRelease(grabScript);
                //POSSIBLE AUDIO: some sort of short "cable pulled taut" noise
            }
        }
    }

    private void CheckDistanceBetweenCapsules()
    {
        if (backNeighborCapsule)
        {
            backDistance = Vector3.Distance(transform.position, backNeighborCapsule.transform.position);
            if (backDistance > maxLengthBetweenCapsules)
            {
                UngrabCable();
            }
        }

        if (frontNeighborCapsule)
        {
            frontDistance = Vector3.Distance(transform.position, frontNeighborCapsule.transform.position);
            if (frontDistance > maxLengthBetweenCapsules)
            {
                UngrabCable();
            }
        }

    }
}
