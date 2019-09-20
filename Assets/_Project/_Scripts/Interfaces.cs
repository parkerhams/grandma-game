using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Signal { Video, LeftAudio, RightAudio, Power, None }; //Denotes the type of signal a cable carries, including no signal
enum CableType { Power, RCA };
public interface ICanHoldSignal
{
    //Signal signal { get; set; }
}