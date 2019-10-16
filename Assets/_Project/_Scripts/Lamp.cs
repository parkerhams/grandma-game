using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{

    public SocketBehavior powerSocket;
    public Light lampLight;

    bool isOn = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (powerSocket.signal == SocketBehavior.Signal.Power)
        {
            if(!isOn)
            {
                TurnOn();
            }
        }
        else
        {
            if(isOn)
            {
                TurnOff();
            }
        }
    }

    void TurnOn()
    {
        lampLight.enabled = true;
        isOn = true;
    }

    void TurnOff()
    {
        lampLight.enabled = false;
        isOn = false;
    }
}
