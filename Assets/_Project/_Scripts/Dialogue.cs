using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    //this handles all of the dialogue starting
    public string[] grandmaSentences;

    [TextArea(1, 5)]
    public string name = "Grandma";
}
