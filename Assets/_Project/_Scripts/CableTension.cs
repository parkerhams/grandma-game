using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableTension : MonoBehaviour
{
    /*
     * We want this script to know: 
     * -Whether or not the cable is plugged into anything                                           [YEET]
     * -whether the cable is being grabbed or not
     * -which number cable this is (GameObject.name)                                                [YEET]
     * -Total number of capsules on this cable                                                      [YEET]
     * 
     * So, when we detect too much distance between the gameobject.name capsule
     * and either of its nearby capsule
     * If it's plugged in then we unplug it,
     * and when we're unplugging it then we unplug to the closer side to this gripped capsule
     * 
     * if it's not pluged into anything and it gets too much tension, just ungrab
     * OR try and tell if there's two capsules being grabbed, and only ungrab one of those capusles
     */

    [SerializeField]
    private float maxLengthBetweenCapsules;

    [SerializeField]
    public GameObject parentCable;

    [SerializeField]
    private GameObject maleEndCapsule, femaleEndCapsule;

    [SerializeField]
    private Vector3 cableTetherPoint;
    //find neighbor capsule and check the distance betweenthose neighbors in fixed update

    private GameObject frontNeighborCapsule, backNeighborCapsule;

    private PlugBehavior plugBehavior;

    private List<GameObject> capsuleChildren;

    private void Start()
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
                print("frontNeighbor capsule added for " + (index+1) + ", front neighbor is " + frontNeighborCapsule);
            }

            if (index <= capsuleChildren.Count-1)
            {
                backNeighborCapsule = capsuleChildren[index + 1];
                print("backNeighbor capsule added for " + (index+1) + ", back neighbor is " + backNeighborCapsule);
            }
        }

        plugBehavior = capsuleChildren[0].transform.Find("Plug").gameObject.GetComponent<PlugBehavior>();

        //reference capsuleCHildren[index].name for name
        //plugBehavior.isPluggedIn       

        maleEndCapsule = capsuleChildren[0];
        femaleEndCapsule = capsuleChildren[capsuleChildren.Count - 1];
    }

    private void Update()
    {
        if (plugBehavior.isPluggedIn)
        {
            //check distance between capsuleChild[i] and capsuleChild[i+1]
        }
    }

    private void CheckDistanceBetweenCapsules(GameObject neighborCapsule)
    {

    }
}
