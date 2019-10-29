using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableGrabMonitor : MonoBehaviour
{
    /// <summary>
    /// Goal of this script:
    /// Keeps track of its capsule children and whether or not any of them are being grabbed.
    /// If a child is currently grabbed and another grab is attempted on any child using the other hand, this script releases the initial grab before letting the other hand grab.
    /// This will prevent the player from trying to force the cable apart with both hands, so that cable over-tension can only occur if pulling out of a socket.
    /// </summary>


    List<GameObject> capsuleChildren = new List<GameObject>();

    bool anyCapsuleIsGrabbed = false;

    // Start is called before the first frame update
    void Start()
    {
        int children = transform.childCount;
        for (int index = 0; index < children; ++index)
        {
            capsuleChildren.Add(transform.GetChild(index).gameObject);
            //Debug.Log(name + " children: " + transform.GetChild(index).gameObject.name);
        }
    }

    private void MonitorGrabs()//if at any moment you try to grab a second capsule of the cable, release the most recent grab. not sure if the logic of this function makes sense, will need to test.
    {
        foreach (GameObject child in capsuleChildren)
        {
            OVRGrabbable grabScript = child.GetComponent<OVRGrabbable>();
            if (grabScript.isGrabbed)
            {
                if (anyCapsuleIsGrabbed)
                {
                    grabScript.grabbedBy.GetComponent<OVRGrabber>().ForceRelease(grabScript);
                    anyCapsuleIsGrabbed = false;
                }
                anyCapsuleIsGrabbed = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        MonitorGrabs();
    }
}
