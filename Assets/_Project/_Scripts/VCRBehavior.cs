using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VCRBehavior : MonoBehaviour
{
    GameObject currentVHS;//the VHS that's currently inside the VCR

    public CRTBehavior CRTBehaviorScript;

    public GameObject flapUp;//having two separate game objects for the flap in its different positions was easier than rotating/moving it
    public GameObject flapDown;
    public GameObject entryPosition;//where the VHS will jump to when it's getting ready to move inside the VCR
    public GameObject insidePosition;//where the VHS will be when it's fully inside the VCR

    public Text debugText;

    [SerializeField]
    public TextMeshProUGUI grandmaText;

    float waitTime = 1.5f;//when a VHS is accepted or ejected, don't accept or remove any VHS for this duration
    bool isWaiting = false;//set to true while the VHS has recently accepted or ejected a VHS

    AudioSource audioSource;

    private void Start()
    {
        flapUp.SetActive(false);//if this flap isn't disabled in scene view, disable it now
        debugText.text = "No VHS";

        if (!GetComponent<AudioSource>())
        {
            Debug.Log("No audio source component on " + gameObject.name + "! It needs one!");
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "DistanceGrabHandLeft Variant" || other.gameObject.name == "DistanceGrabHandRight Variant") //if a player hand enters the button trigger
        {
            EjectVHS();
        }
        else if(other.GetComponent<VHSBehavior>())
        {
            AcceptVHS(other.gameObject);
        }
    }

    void MoveFlap(bool up)//the flap of the VCR that gets pushed up as you put a VHS in
    {
        if(up)
        {
            flapUp.SetActive(true);
            flapDown.SetActive(false);
        }
        else
        {
            flapUp.SetActive(false);
            flapDown.SetActive(true);
        }
    }

    void EjectVHS()
    {
        if (isWaiting)
        {
            return;
        }
        if (!currentVHS)
        {
            //debugText.text = "no current VHS";
            return;//can't eject a VHS if there isn't one in it
        }
        StartCoroutine(VHSWaitCoroutine());
        StartCoroutine(VHSMovementCoroutine(false));

    }

    void AcceptVHS(GameObject VHS)
    {
        grandmaText.text = "Oh, I can't wait to watch your performance!";
        
        if (currentVHS)
        {
            return;//can't take a VHS if there's already one in it
        }
        if (isWaiting)
        {
            return;
        }
        //ungrab VHS
        if (VHS.GetComponent<OVRGrabbable>().isGrabbed)
        {
            OVRGrabbable grabScript = VHS.GetComponent<OVRGrabbable>();
            grabScript.grabbedBy.ForceRelease(grabScript);
        }
        Rigidbody rb = VHS.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        VHS.transform.position = entryPosition.transform.position;
        VHSBehavior VHSscript = VHS.GetComponent<VHSBehavior>();
        //if(VHSscript.rotationToFitVCR != null)
        //{
        //    VHS.transform.rotation = VHSscript.rotationToFitVCR;
        //}
        //else
        //{
        //    VHS.transform.rotation = entryPosition.transform.rotation;
        //}
        VHS.transform.rotation = entryPosition.transform.rotation;
        StartCoroutine(VHSWaitCoroutine());
        currentVHS = VHS;
        StartCoroutine(VHSMovementCoroutine(true));

    }

    IEnumerator VHSMovementCoroutine(bool goingIn)
    {
        float timeWaited = 0;
        float duration = .7f;
        float step = .6f * Time.deltaTime;
        currentVHS.GetComponent<OVRGrabbable>().m_allowGrab = false;
        MoveFlap(true);
        while(timeWaited < duration)
        {
            timeWaited += Time.deltaTime;
            if(goingIn)
            {
                currentVHS.transform.position = Vector3.MoveTowards(currentVHS.transform.position, insidePosition.transform.position, step);
            }
            else
            {
                currentVHS.transform.position = Vector3.MoveTowards(currentVHS.transform.position, entryPosition.transform.position, step);
            }
            yield return null;
        }
        MoveFlap(false);
        if(goingIn)
        {
            currentVHS.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            CRTBehaviorScript.currentVHS = currentVHS;
            debugText.text = currentVHS.name;
        }
        else
        {
            currentVHS.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            //re-enable grabbing of VHS
            currentVHS.GetComponent<OVRGrabbable>().m_allowGrab = false;
            Rigidbody rb = currentVHS.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            currentVHS = null;
            CRTBehaviorScript.currentVHS = null;
            debugText.text = "VHS removed";
        }
    }

    IEnumerator VHSWaitCoroutine()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }
}
