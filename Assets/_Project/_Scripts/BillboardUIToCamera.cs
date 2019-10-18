using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;

public class BillboardUIToCamera : MonoBehaviour
{
    [SerializeField]
    public OVRCameraRig playerCamRig;
 
    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        transform.LookAt(transform.position + playerCamRig.transform.rotation * Vector3.forward,
            playerCamRig.transform.rotation * Vector3.up);
    }
}
