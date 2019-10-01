using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        debugGrabbability();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void debugGrabbability()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
}
