using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableTension : MonoBehaviour
{
    /*
     * We want this script to know: 
     * -Whether or not the cable is plugged into anything
     * -whether the cable is being grabbed or not
     * -which number cable this is (GameObject.name)
     * -Total number of capsules on this cable                                                      [YEET]
     * 
     * So, when we detect too much distance between the gameobject.name capsule
     * and either of its nearby capsule (name.ToInt32(+1, -1))
     * If it's plugged in then we unplug it,
     * and when we're unplugging it then we unplug to the closer side to this gripped capsule
     * 
     * if it's not pluged into anything and it gets too much tension, just ungrab
     * OR try and tell if there's two capsules being grabbed, and only ungrab one of those capusles
     */

    [SerializeField]
    private Vector3 maxDistance;

    [SerializeField]
    public GameObject parentCable;

    [SerializeField]
    private GameObject maleEndCapsule, femaleEndCapsule;

    private PlugBehavior plugBehaviorObject;

    private List<GameObject> capsuleChildren;

    private void Start()
    {
        plugBehaviorObject = this.gameObject.GetComponentInParent<PlugBehavior>();

        maxDistance = new Vector3(.5f, .5f, .5f);
        capsuleChildren = new List<GameObject>();

        int children = parentCable.transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            capsuleChildren.Add(parentCable.transform.GetChild(i).gameObject);
            print("For loop: " + transform.GetChild(i));

        }

        maleEndCapsule = capsuleChildren[0];
        femaleEndCapsule = capsuleChildren[capsuleChildren.Count - 1];
    }


}
