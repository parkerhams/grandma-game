using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    //this handles all of the dialogue starting
    public string[] grandmaSentences;

    //min 1 text line, max 10 text lines
    [TextArea(1, 10)]
    public string name = "Grandma";
}
