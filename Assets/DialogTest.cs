using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PixelCrushers.DialogueSystem.DialogueManager.StartConversation("PlayerPicksUpBanana");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
