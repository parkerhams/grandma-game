using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSBehavior : MonoBehaviour
{
    public Vector3 fixedRotation;
    //public Quaternion rotationToFitVCR = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void FixRotation()
    {
        if(fixedRotation == null)
        {
            return;
        }
        Quaternion a = Quaternion.Euler(fixedRotation);
        transform.rotation = a;
    }
}
